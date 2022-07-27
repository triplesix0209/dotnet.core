using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using TripleSix.Core.Jsons;
using TripleSix.Core.WebApi;

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
        /// Danh sách Model Binder Provider mặc định.
        /// </summary>
        public static readonly IModelBinderProvider[] ModelBinderProviders = new[]
        {
            new TimestampModelBinderProvider(),
        };

        /// <summary>
        /// Cấu hình Json serializer mặc đình.
        /// </summary>
        public static readonly JsonSerializerSettings SerializerSettings = new()
        {
            ContractResolver = new BaseContractResolver(),
            Converters = Converters,
        };

        /// <summary>
        /// Mã hóa đối tượng thành chuỗi JSON.
        /// </summary>
        /// <param name="value">Đối tượng sẽ được mã hóa.</param>
        /// <returns>Chuỗi JSON ứng với đối tượng chỉ định.</returns>
        public static string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value, SerializerSettings);
        }

        /// <summary>
        /// Mã hóa đối tượng thành chuỗi JSON.
        /// </summary>
        /// <param name="value">Đối tượng sẽ được mã hóa.</param>
        /// <param name="ignorePropertyNames">Danh sách property loại bỏ.</param>
        /// <returns>Chuỗi JSON ứng với đối tượng chỉ định.</returns>
        public static string SerializeObject(object value, params string[] ignorePropertyNames)
        {
            return JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                ContractResolver = new IgnoreContractResolver(ignorePropertyNames),
                Converters = Converters,
            });
        }
    }
}
