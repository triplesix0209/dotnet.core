using Microsoft.Extensions.Configuration;

namespace TripleSix.Core.Appsettings
{
    /// <summary>
    /// Cấu hình Web api.
    /// </summary>
    public class WebApiAppsetting : BaseAppsetting
    {
        /// <summary>
        /// Cấu hình Web api.
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        public WebApiAppsetting(IConfiguration configuration)
            : base(configuration, "WebApi")
        {
        }

        /// <summary>
        /// Url công khai của api.
        /// </summary>
        public string PublicUrl { get; set; }

        /// <summary>
        /// Hiển thị stack trace.
        /// </summary>
        public bool ShowStackTrace { get; set; } = false;

        /// <summary>
        /// Danh sách domain cho phép request.
        /// </summary>
        public string[] AllowedOrigins { get; set; } = new[] { "*" };
    }
}
