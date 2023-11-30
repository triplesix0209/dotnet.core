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
        /// <param name="setting"><see cref="ElasticsearchAppsetting"/>.</param>
        /// <param name="assembly"><see cref="Assembly"/> chứa các elastic documents.</param>
        /// <param name="putTemplateOption">Cấu hình template mặc định.</param>
        /// <returns>Task xử lý.</returns>
        public static async Task SetupElasticsearch(
            this IHost app,
            ElasticsearchAppsetting setting,
            Assembly assembly,
            Action<PutTemplateRequest, ElasticsearchTemplateAppsetting>? putTemplateOption = null)
        {
            var logger = app.Services.GetService(typeof(ILogger<ElasticsearchClient>)) as ILogger<ElasticsearchClient>;
            var client = CreateElasticsearchClient(setting);
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
            var templateConfig = setting.Template;
            if (templateConfig != null)
            {
                var pattern = templateConfig.Pattern;
                if (!pattern.EndsWith("*")) pattern += "*";
                var templateRequest = new PutTemplateRequest("default")
                {
                    IndexPatterns = new[] { pattern },
                    Settings = new Dictionary<string, object>(),
                };

                templateRequest.Settings.Add("index.analysis", new Dictionary<string, object>()
                {
                    {
                        "analyzer", new Dictionary<string, object>()
                        {
                            {
                                "vi_analyzer", new Dictionary<string, object>()
                                {
                                    { "tokenizer", "icu_tokenizer" },
                                    { "filter", new[] { "icu_folding", "lowercase" } },
                                }
                            },
                        }
                    },
                });

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
                    putMappingRequest.Properties.Add(property.Name.ToCamelCase(), property.PropertyType.ElasticPropertyType(setting));
                response = await client.Indices.PutMappingAsync(putMappingRequest);
                logger.LogElasticResponse(response, $"Update mapping [{indexName}]");
            }
        }

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
            await SetupElasticsearch(app, new ElasticsearchAppsetting(configuration), assembly, putTemplateOption);
        }

        internal static ElasticsearchClient CreateElasticsearchClient(ElasticsearchAppsetting setting)
        {
            var clientSetting = new ElasticsearchClientSettings(new Uri(setting.Host));
            if (setting.Username.IsNotNullOrEmpty() && setting.Password.IsNotNullOrEmpty())
                clientSetting.Authentication(new BasicAuthentication(setting.Username, setting.Password));
            return new ElasticsearchClient(clientSetting);
        }

        internal static ElasticsearchClient CreateElasticsearchClient(IConfiguration configuration)
        {
            return CreateElasticsearchClient(new ElasticsearchAppsetting(configuration));
        }

        private static void LogElasticResponse(this ILogger? logger, ElasticsearchResponse response, string message)
        {
            if (logger == null) return;

            if (response.IsValidResponse)
                logger.LogInformation($"{message}... Success");
            else
                logger.LogError($"{message}... Failed" + (response.ElasticsearchServerError != null ? "\n" + response.ElasticsearchServerError.Error.Reason : string.Empty));
        }

        private static IProperty ElasticPropertyType(this Type type, ElasticsearchAppsetting setting, int level = 0)
        {
            if (level > setting.MaxDepthPropertyAllowed)
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

            if (dataType == typeof(Guid))
                return new KeywordProperty();
            if (dataType.IsEnum)
                return new KeywordProperty();

            if (dataType == typeof(string))
                return new TextProperty { Analyzer = "vi_analyzer" };

            if (dataType.IsClass || dataType.IsAssignableToGenericType(typeof(ICollection<>)))
            {
                var result = new ObjectProperty() { Properties = new Properties() };

                Type? itemType = null;
                if (dataType.IsArray) itemType = dataType.GetElementType();
                else if (dataType.IsAssignableToGenericType(typeof(ICollection<>))) itemType = dataType.GetGenericArguments()[0];

                var listProperty = itemType != null ? itemType.GetProperties() : dataType.GetProperties();
                foreach (var property in listProperty)
                    result.Properties.Add(property.Name.ToCamelCase(), property.PropertyType.ElasticPropertyType(setting, level + 1));

                return result;
            }

            throw new Exception($"Unsupport mapping for {dataType.Name}");
        }
    }
}
