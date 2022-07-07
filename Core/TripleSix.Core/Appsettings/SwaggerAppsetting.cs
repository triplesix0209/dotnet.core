using Microsoft.Extensions.Configuration;

namespace TripleSix.Core.Appsettings
{
    public class SwaggerAppsetting : BaseAppsetting
    {
        public SwaggerAppsetting(IConfiguration configuration)
            : base(configuration, "Swagger")
        {
        }

        /// <summary>
        /// Bật/tắt swagger.
        /// </summary>
        public bool Enable { get; set; } = true;

        /// <summary>
        /// Route đến swagger.
        /// </summary>
        public string Route { get; set; } = "swagger";
    }
}
