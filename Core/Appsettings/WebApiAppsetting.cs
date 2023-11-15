using Microsoft.Extensions.Configuration;

namespace TripleSix.Core.Appsettings
{
    public class WebApiAppsetting : BaseAppsetting
    {
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
