using System.ComponentModel;
using Newtonsoft.Json;

namespace TripleSix.Core.WebApi
{
    public class DataError<TData> : BaseError
    {
        public DataError(string code, string message)
            : base(code, message)
        {
            Data = default;
        }

        public DataError(string code, string message, TData? data)
            : base(code, message)
        {
            Data = data;
        }

        [JsonProperty(Order = -9)]
        [DisplayName("dữ liệu lỗi")]
        public TData? Data { get; set; }
    }
}
