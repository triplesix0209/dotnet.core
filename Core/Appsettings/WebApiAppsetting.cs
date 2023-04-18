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
        /// Hiển thị chi tiết lỗi.
        /// </summary>
        public bool ShowErrorDetail { get; set; } = false;
    }
}
