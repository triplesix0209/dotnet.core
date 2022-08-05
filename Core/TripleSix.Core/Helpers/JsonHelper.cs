using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TripleSix.Core.Jsons;

namespace TripleSix.Core.Helpers
{
    /// <summary>
    /// Helper xử lý json.
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// Danh sách Json Converter mặc định.
        /// </summary>
        public static readonly JsonConverter[] Converters = new[]
        {
            new TimestampConverter(),
        };

        /// <summary>
        /// Cấu hình Json Serializer mặc đình.
        /// </summary>
        public static readonly JsonSerializerSettings SerializerSettings = new()
        {
            ContractResolver = new BaseContractResolver(),
            Converters = Converters,
        };

        /// <summary>
        /// Mã hóa đối tượng thành chuỗi JSON.
        /// </summary>
        /// <param name="obj">Đối tượng sẽ được mã hóa.</param>
        /// <returns>Chuỗi JSON ứng với đối tượng chỉ định.</returns>
        public static string? ToJson([NotNullWhen(false)] this object? obj)
        {
            if (obj == null) return null;
            return JsonConvert.SerializeObject(obj, SerializerSettings);
        }

        /// <summary>
        /// Mã hóa đối tượng thành chuỗi JSON.
        /// </summary>
        /// <param name="obj">Đối tượng sẽ được mã hóa.</param>
        /// <param name="ignorePropertyNames">Danh sách property loại bỏ.</param>
        /// <returns>Chuỗi JSON ứng với đối tượng chỉ định.</returns>
        public static string? ToJson([NotNullWhen(false)] this object? obj, params string[] ignorePropertyNames)
        {
            if (obj == null) return null;
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = new IgnoreContractResolver(ignorePropertyNames),
                Converters = Converters,
            });
        }

        /// <summary>
        /// Chuyển đổi chuổi JSON thành JToken.
        /// </summary>
        /// <param name="json">Chuổi JSON cần đọc.</param>
        /// <returns><see cref="JToken"/>.</returns>
        public static JToken? ToJToken([NotNullWhen(false)] this string? json)
        {
            if (json.IsNullOrEmpty()) return null;
            return JsonConvert.DeserializeObject<JToken>(json, SerializerSettings);
        }

        /// <summary>
        /// Chuyển đổi chuổi JSON thành đối tượng.
        /// </summary>
        /// <param name="json">Chuổi Json cần đọc.</param>
        /// <param name="type">Loại đối tượng.</param>
        /// <returns>Đối tượng được chuyển đổi từ chuỗi JSON.</returns>
        public static object? ToObject([NotNullWhen(false)] this string? json, Type type)
        {
            if (json.IsNullOrEmpty()) return null;
            return JsonConvert.DeserializeObject(json, type, SerializerSettings);
        }

        /// <summary>
        /// Chuyển đổi chuổi JSON thành đối tượng.
        /// </summary>
        /// <typeparam name="T">Loại đối tượng.</typeparam>
        /// <param name="json">Chuổi Json cần đọc.</param>
        /// <returns>Đối tượng được chuyển đổi từ chuỗi JSON.</returns>
        public static T? ToObject<T>([NotNullWhen(false)] this string json)
        {
            if (json.IsNullOrEmpty()) return default;
            return JsonConvert.DeserializeObject<T>(json, SerializerSettings);
        }
    }
}
