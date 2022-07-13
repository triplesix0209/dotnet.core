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
        /// Hiển thị lỗi đầy đủ của <see cref="Exception"/>.
        /// </summary>
        public bool DisplayUnexpectedException { get; set; } = false;
    }
}
