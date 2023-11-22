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
        /// Hiển thị stack trace.
        /// </summary>
        public bool ShowStackTrace { get; set; } = false;
    }
}
