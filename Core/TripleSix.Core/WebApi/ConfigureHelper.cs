using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TripleSix.Core.Appsettings;
using TripleSix.Core.Helpers;
using TripleSix.Core.Jsons;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Helper hỗ trợ cấu hình Web Api.
    /// </summary>
    public static class ConfigureHelper
    {
        /// <summary>
        /// Cấu hình MVC Controller.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>.</param>
        /// <returns><see cref="IMvcBuilder"/>.</returns>
        public static IMvcBuilder ConfigureMvcService(this IServiceCollection services)
        {
            return services
                .AddMvc(options =>
                {
                    options.AllowEmptyInputInBodyModelBinding = true;
                    foreach (var modelBinderProviders in JsonHelper.ModelBinderProviders)
                        options.ModelBinderProviders.Insert(0, modelBinderProviders);
                })
                .AddControllersAsServices()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new BaseContractResolver();
                    options.SerializerSettings.Converters.Add(new DtoConverter());
                    foreach (var converter in JsonHelper.Converters)
                        options.SerializerSettings.Converters.Add(converter);
                });
        }

        /// <summary>
        /// Cấu hình Swagger.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        /// <param name="setupAction">Hàm tùy chỉnh Swagger.</param>
        /// <returns><see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration, Action<SwaggerGenOptions, SwaggerAppsetting>? setupAction = null)
        {
            var appsetting = new SwaggerAppsetting(configuration);
            if (!appsetting.Enable) return services;

            return services.AddSwaggerGen(options =>
            {
                options.SwaggerGeneratorOptions.DescribeAllParametersInCamelCase = true;
                options.CustomSchemaIds(x => x.FullName);
                options.EnableAnnotations();

                options.MapType<DateTime>(() => new OpenApiSchema { Type = "integer", Format = "int64" });
                options.MapType<DateTime?>(() => new OpenApiSchema { Type = "integer", Format = "int64", Nullable = true });

                options.DocumentFilter<BaseDocumentFilter>();

                if (setupAction != null) setupAction(options, appsetting);
            });
        }
    }
}
