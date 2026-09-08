using MQTTnet;
using MQTTnet.Protocol;

namespace TripleSix.Core.Mqtt
{
    /// <summary>
    /// Lớp định nghĩa cấu hình topic MQTT, hàm xử lý và mức QoS.
    /// </summary>
    /// <param name="topic">Tên topic MQTT.</param>
    /// <param name="handler">Hàm xử lý bản tin MQTT.</param>
    /// <param name="qualityOfServiceLevel">Mức chất lượng dịch vụ QoS.</param>
    public class MqttTopicHandler(
        string topic,
        Func<MqttApplicationMessage, CancellationToken, Task> handler,
        MqttQualityOfServiceLevel qualityOfServiceLevel = MqttQualityOfServiceLevel.AtMostOnce)
    {
        /// <summary>
        /// Hàm xử lý bản tin MQTT.
        /// </summary>
        public Func<MqttApplicationMessage, CancellationToken, Task> Handler { get; set; } = handler;

        /// <summary>
        /// Mức chất lượng dịch vụ QoS.
        /// </summary>
        public MqttQualityOfServiceLevel QualityOfServiceLevel { get; set; } = qualityOfServiceLevel;

        /// <summary>
        /// Tên topic MQTT.
        /// </summary>
        public string Topic { get; set; } = topic;

        /// <summary>
        /// Kiểm tra xem topic nhận được có khớp với topic cấu hình hay không (hỗ trợ cả wildcard MQTT + và #).
        /// </summary>
        /// <param name="topic">Tên topic cần kiểm tra.</param>
        /// <returns><c>true</c> nếu khớp; ngược lại <c>false</c>.</returns>
        public virtual bool IsMatchTopic(string topic)
        {
            if (string.Equals(Topic, topic, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!Topic.Contains('+') && !Topic.Contains('#'))
                return false;

            var patternSegments = Topic.Split('/');
            var topicSegments = topic.Split('/');

            for (var i = 0; i < patternSegments.Length; i++)
            {
                var pattern = patternSegments[i];
                if (pattern == "#")
                    return true;

                if (i >= topicSegments.Length)
                    return false;

                if (pattern != "+" && !string.Equals(pattern, topicSegments[i], StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            return patternSegments.Length == topicSegments.Length;
        }
    }
}
