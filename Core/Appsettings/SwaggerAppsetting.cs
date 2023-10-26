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

        /// <summary>
        /// Tiêu đề của API Document.
        /// </summary>
        public string Title { get; set; } = "API Document";

        /// <summary>
        /// Version của API Document.
        /// </summary>
        public string Version { get; set; } = string.Empty;
    }
}
