namespace TripleSix.Core.Attributes
{
    /// <summary>
    /// Đánh dấu MQTT topic mà phương thức xử lý nhận bản tin.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MqttTopicAttribute : Attribute
    {
        /// <summary>
        /// Khởi tạo thuộc tính MqttTopic.
        /// </summary>
        /// <param name="topic">Tên topic MQTT.</param>
        public MqttTopicAttribute(string topic)
        {
            Topic = topic;
        }

        /// <summary>
        /// Tên topic MQTT.
        /// </summary>
        public string Topic { get; }
    }
}
