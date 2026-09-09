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
        public static readonly JsonConverter[] Converters =
        [
            new TimestampConverter(),
        ];

        /// <summary>
        /// Cấu hình Json Serializer mặc định.
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
        public static string ToJsonText(this object obj)
        {
            return JsonConvert.SerializeObject(obj, SerializerSettings);
        }

        /// <summary>
        /// Mã hóa đối tượng thành chuỗi JSON.
        /// </summary>
        /// <param name="obj">Đối tượng sẽ được mã hóa.</param>
        /// <param name="ignorePropertyNames">Danh sách property loại bỏ.</param>
        /// <returns>Chuỗi JSON ứng với đối tượng chỉ định.</returns>
        public static string ToJsonText(this object obj, params string[] ignorePropertyNames)
        {
            if (ignorePropertyNames == null || ignorePropertyNames.Length == 0)
                return obj.ToJsonText();

            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = new IgnoreContractResolver(ignorePropertyNames),
                Converters = Converters,
            });
        }

        /// <summary>
        /// Chuyển đổi chuỗi JSON thành JToken.
        /// </summary>
        /// <param name="json">Chuỗi JSON cần đọc.</param>
        /// <returns><see cref="JToken"/>.</returns>
        public static JToken? ToJToken(this string json)
        {
            if (json.IsNullOrEmpty()) return null;
            return JsonConvert.DeserializeObject<JToken>(json, SerializerSettings);
        }

        /// <summary>
        /// Chuyển đổi object thành JToken.
        /// </summary>
        /// <param name="obj">Object cần đọc.</param>
        /// <returns><see cref="JToken"/>.</returns>
        public static JToken? ToJToken(this object obj)
        {
            if (obj == null) return null;
            return JToken.FromObject(obj, JsonSerializer.Create(SerializerSettings));
        }

        /// <summary>
        /// Chuyển đổi chuỗi JSON thành đối tượng.
        /// </summary>
        /// <param name="json">Chuỗi JSON cần đọc.</param>
        /// <param name="type">Loại đối tượng.</param>
        /// <returns>Đối tượng được chuyển đổi từ chuỗi JSON.</returns>
        public static object? ToObject(this string json, Type type)
        {
            if (json.IsNullOrEmpty()) return null;
            return JsonConvert.DeserializeObject(json, type, SerializerSettings);
        }

        /// <summary>
        /// Chuyển đổi chuỗi JSON thành đối tượng.
        /// </summary>
        /// <typeparam name="T">Loại đối tượng.</typeparam>
        /// <param name="json">Chuỗi JSON cần đọc.</param>
        /// <returns>Đối tượng được chuyển đổi từ chuỗi JSON.</returns>
        public static T? ToObject<T>(this string json)
        {
            if (json.IsNullOrEmpty()) return default;
            return JsonConvert.DeserializeObject<T>(json, SerializerSettings);
        }

        /// <summary>
        /// Chuyển đổi JToken thành đối tượng.
        /// </summary>
        /// <typeparam name="T">Loại đối tượng.</typeparam>
        /// <param name="token"><see cref="JToken"/>.</param>
        /// <returns>Đối tượng được chuyển đổi.</returns>
        public static T? ToObject<T>(this JToken token)
        {
            if (token == null) return default;
            return token.ToObject<T>(JsonSerializer.Create(SerializerSettings));
        }

        /// <summary>
        /// Chuyển đổi JToken thành đối tượng.
        /// </summary>
        /// <param name="token"><see cref="JToken"/>.</param>
        /// <param name="type">Loại đối tượng.</param>
        /// <returns>Đối tượng được chuyển đổi.</returns>
        public static object? ToObject(this JToken token, Type type)
        {
            if (token == null) return null;
            return token.ToObject(type, JsonSerializer.Create(SerializerSettings));
        }
    }
}
