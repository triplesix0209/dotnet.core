using Microsoft.Extensions.Configuration;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Appsettings
{
    /// <summary>
    /// Cấu hình OpenTelemetry.
    /// </summary>
    public class OpenTelemetryAppsetting : BaseAppsetting
    {
        /// <summary>
        /// Cấu hình OpenTelemetry.
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        public OpenTelemetryAppsetting(IConfiguration configuration)
            : base(configuration, "OpenTelemetry")
        {
            if (Enable && Host.IsNullOrEmpty())
                throw new ArgumentException(nameof(Host));

            if (Enable && ServiceName.IsNullOrEmpty())
                throw new ArgumentException(nameof(ServiceName));
        }

        /// <summary>
        /// Bật/tắt OpenTelemetry (mặc định là false).
        /// </summary>
        public bool Enable { get; set; } = false;

        /// <summary>
        /// Service Name.
        /// </summary>
        public string? ServiceName { get; set; }

        /// <summary>
        /// Host nhận trace.
        /// </summary>
        public string? Host { get; set; }

        /// <summary>
        /// Đính kèm log vào trace.
        /// </summary>
        public bool AttachLog { get; set; } = true;
    }
}
