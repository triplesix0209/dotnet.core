using Microsoft.Extensions.DependencyInjection;
using TripleSix.Core.Jsons;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Helper hỗ trợ cấu hình Web Api
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
                    options.ModelBinderProviders.Insert(0, new TimestampModelBinderProvider());
                    options.AllowEmptyInputInBodyModelBinding = true;
                })
                .AddControllersAsServices()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new BaseContractResolver();
                    options.SerializerSettings.Converters.Add(new DtoConverter());
                    options.SerializerSettings.Converters.Add(new TimestampConverter());
                });
        }
    }
}
