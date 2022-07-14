using Microsoft.Extensions.Configuration;
using TripleSix.Core.Appsettings;

namespace Sample.Infrastructure.Appsettings
{
    public class OpenTelemetryAppsetting : BaseAppsetting
    {
        public OpenTelemetryAppsetting(IConfiguration configuration)
            : base(configuration, "OpenTelemetry")
        {
            if (ServiceName is null) throw new ArgumentNullException(nameof(ServiceName));
        }

        /// <summary>
        /// Tên service.
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Phiên bản của service.
        /// </summary>
        public string ServiceVersion { get; set; } = "1.0.0";
    }
}
