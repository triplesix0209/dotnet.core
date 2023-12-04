using System.ComponentModel;
using Newtonsoft.Json;

namespace TripleSix.CoreOld.WebApi.Results
{
    public class DataError<TData> : BaseError
    {
        [JsonProperty(Order = -9)]
        [DisplayName("thông tin lỗi")]
        public TData Data { get; set; }
    }
}