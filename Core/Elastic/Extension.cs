using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.IndexManagement;
using Elastic.Clients.Elasticsearch.Mapping;
using Elastic.Transport;
using Elastic.Transport.Products.Elasticsearch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TripleSix.Core.Appsettings;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Elastic
{
    /// <summary>
    /// Extension.
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// Thiết lập Elasticsearch.
        /// </summary>
        /// <param name="app"><see cref="IHost"/>.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        /// <param name="assembly"><see cref="Assembly"/> chứa các elastic documents.</param>
        /// <param name="putTemplateOption">Cấu hình template mặc định.</param>
        /// <returns>Task xử lý.</returns>
        public static async Task SetupElasticsearch(
            this IHost app,
            IConfiguration configuration,
            Assembly assembly,
            Action<PutTemplateRequest, ElasticsearchTemplateAppsetting>? putTemplateOption = null)
        {
            var logger = app.Services.GetService(typeof(ILogger<ElasticsearchClient>)) as ILogger<ElasticsearchClient>;
            var config = new ElasticsearchAppsetting(configuration);
            var client = CreateElasticsearchClient(configuration);
            var documentTypes = assembly.GetExportedTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => x.GetCustomAttribute<ElasticDocumentAttribute>() != null);
            ElasticsearchResponse response;

            // validate
            foreach (var documentType in documentTypes)
            {
                var properties = documentType.GetProperties();
                if (!properties.Any(x => x.GetCustomAttribute<KeyAttribute>() != null))
                    throw new Exception($"{documentType.Name} is missing [Key] property");
            }

            // put template
            var templateConfig = config.Template;
            if (templateConfig != null)
            {
                var pattern = templateConfig.Pattern;
                if (!pattern.EndsWith("*")) pattern += "*";
                var templateRequest = new PutTemplateRequest("default")
                {
                    IndexPatterns = new[] { pattern },
                    Settings = new Dictionary<string, object>(),
                };

                if (templateConfig.NumberOfReplicas.HasValue)
                    templateRequest.Settings.Add("index.number_of_replicas", templateConfig.NumberOfReplicas);
                putTemplateOption?.Invoke(templateRequest, templateConfig);

                response = await client.Indices.PutTemplateAsync(templateRequest);
                logger.LogElasticResponse(response, $"Update default template [{templateConfig.Pattern}]");
            }

            // setup documents
            foreach (var documentType in documentTypes)
            {
                var attr = documentType.GetCustomAttribute<ElasticDocumentAttribute>();
                if (attr == null) continue;
                var indexName = attr.FullIndexName();

                // create index
                var existsResponse = await client.Indices.ExistsAsync(indexName);
                if (!existsResponse.Exists)
                {
                    response = await client.Indices.CreateAsync(indexName);
                    logger.LogElasticResponse(response, $"Create document [{indexName}]");
                }

                // put mapping
                var putMappingRequest = new PutMappingRequest(indexName) { Properties = new Properties() };
                foreach (var property in documentType.GetProperties())
                    putMappingRequest.Properties.Add(property.Name.ToCamelCase(), property.PropertyType.ElasticPropertyType(config));
                response = await client.Indices.PutMappingAsync(putMappingRequest);
                logger.LogElasticResponse(response, $"Update mapping [{indexName}]");
            }
        }

        internal static ElasticsearchClient CreateElasticsearchClient(IConfiguration configuration)
        {
            var config = new ElasticsearchAppsetting(configuration);
            var setting = new ElasticsearchClientSettings(new Uri(config.Host));
            if (config.Username.IsNotNullOrEmpty() && config.Password.IsNotNullOrEmpty())
                setting.Authentication(new BasicAuthentication(config.Username, config.Password));
            return new ElasticsearchClient(setting);
        }

        private static void LogElasticResponse(this ILogger? logger, ElasticsearchResponse response, string message)
        {
            if (logger == null) return;

            if (response.IsValidResponse)
                logger.LogInformation($"{message}... Success");
            else
                logger.LogError($"{message}... Failed" + (response.ElasticsearchServerError != null ? "\n" + response.ElasticsearchServerError.Error.Reason : string.Empty));
        }

        private static IProperty ElasticPropertyType(this Type type, ElasticsearchAppsetting config, int level = 0)
        {
            if (level > config.MaxDepthPropertyAllowed)
                throw new Exception("Max depth property reach");

            var dataType = type.GetUnderlyingType();

            if (dataType == typeof(bool))
                return new BooleanProperty();

            if (dataType == typeof(byte) || dataType == typeof(sbyte))
                return new ByteNumberProperty();
            if (dataType == typeof(int) || dataType == typeof(uint))
                return new IntegerNumberProperty();
            if (dataType == typeof(short) || dataType == typeof(ushort))
                return new ShortNumberProperty();
            if (dataType == typeof(long))
                return new LongNumberProperty();
            if (dataType == typeof(ulong))
                return new UnsignedLongNumberProperty();
            if (dataType == typeof(float))
                return new FloatNumberProperty();
            if (dataType == typeof(double) || dataType == typeof(decimal))
                return new DoubleNumberProperty();

            if (dataType == typeof(DateTime))
                return new DateProperty();

            if (dataType == typeof(string))
                return new TextProperty();

            if (dataType == typeof(Guid))
                return new KeywordProperty();
            if (dataType.IsEnum)
                return new KeywordProperty();

            if (dataType.IsClass || dataType.IsAssignableToGenericType(typeof(ICollection<>)))
            {
                var result = new ObjectProperty() { Properties = new Properties() };

                Type? itemType = null;
                if (dataType.IsArray) itemType = dataType.GetElementType();
                else if (dataType.IsAssignableToGenericType(typeof(ICollection<>))) itemType = dataType.GetGenericArguments()[0];

                var listProperty = itemType != null ? itemType.GetProperties() : dataType.GetProperties();
                foreach (var property in listProperty)
                    result.Properties.Add(property.Name.ToCamelCase(), property.PropertyType.ElasticPropertyType(config, level + 1));

                return result;
            }

            throw new Exception($"Unsupport mapping for {dataType.Name}");
        }
    }
}
