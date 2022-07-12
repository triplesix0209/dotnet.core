using Newtonsoft.Json;

namespace TripleSix.Core.Jsons
{
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
    }
}
