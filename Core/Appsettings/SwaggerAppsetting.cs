using Microsoft.Extensions.Configuration;

namespace TripleSix.Core.Appsettings
{
    /// <summary>
    /// Cấu hình swagger.
    /// </summary>
    public class SwaggerAppsetting : BaseAppsetting
    {
        /// <summary>
        /// Cấu hình swagger.
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
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
