using Microsoft.Extensions.Configuration;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Appsettings
{
    /// <summary>
    /// Cấu hình MQTT.
    /// </summary>
    public class MqttAppsetting : BaseAppsetting
    {
        /// <summary>
        /// Khởi tạo cấu hình MQTT.
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        public MqttAppsetting(IConfiguration configuration)
            : base(configuration, "Mqtt")
        {
            if (Enable && Host.IsNullOrEmpty())
                throw new ArgumentException(nameof(Host));
            if (Enable && Port <= 0)
                throw new ArgumentException(nameof(Port));
        }

        /// <summary>
        /// Bật/tắt kết nối MQTT.
        /// </summary>
        public bool Enable { get; set; } = false;

        /// <summary>
        /// Host kết nối MQTT broker.
        /// </summary>
        public string Host { get; set; } = "localhost";

        /// <summary>
        /// Port kết nối MQTT broker (mặc định 1883).
        /// </summary>
        public int Port { get; set; } = 1883;
    }
}
