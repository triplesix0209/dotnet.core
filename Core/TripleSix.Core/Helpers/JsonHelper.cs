using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TripleSix.Core.Jsons;

namespace TripleSix.Core.Helpers
{
    /// <summary>
    /// Helper xử lý json.
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// Cấu hình Json serializer mặc đình.
        /// </summary>
        public static readonly JsonSerializerSettings SerializerSettings = new ()
        {
            ContractResolver = new BaseContractResolver(),
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
        /// Thêm bộ xử lý json cho IDto.
        /// </summary>
        /// <param name="newtonsoftJsonOptions">Newtonsoft json options.</param>
        public static void AddDtoConverter(this MvcNewtonsoftJsonOptions newtonsoftJsonOptions)
        {
            newtonsoftJsonOptions.SerializerSettings.ContractResolver = new BaseContractResolver();
            newtonsoftJsonOptions.SerializerSettings.Converters.Add(new DtoConverter());
        }
    }
}
