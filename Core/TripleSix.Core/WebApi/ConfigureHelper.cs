using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TripleSix.Core.Appsettings;
using TripleSix.Core.AutoAdmin;
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
        /// <param name="assembly">Assembly chứa các controller.</param>
        /// <returns><see cref="IMvcBuilder"/>.</returns>
        public static IMvcBuilder ConfigureMvcService(this IServiceCollection services, Assembly assembly)
        {
            return services
                .AddMvc(options =>
                {
                    options.AllowEmptyInputInBodyModelBinding = true;
                    options.ModelBinderProviders.Insert(0, new DtoModelBinderProvider());
                    options.ModelBinderProviders.Insert(0, new TimestampModelBinderProvider());

                    options.Conventions.Add(new AdminControllerRouteConvention(assembly));
                })
                .AddControllersAsServices()
                .ConfigureApplicationPartManager(options =>
                {
                    options.FeatureProviders.Add(new AdminControllerFeatureProvider(assembly));
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new BaseContractResolver();
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
                options.OperationFilter<DescribeOperationFilter>();

                if (setupAction != null) setupAction(options, appsetting);
            });
        }
    }
}
