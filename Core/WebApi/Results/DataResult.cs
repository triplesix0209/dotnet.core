using System.ComponentModel;
using Newtonsoft.Json;

namespace TripleSix.Core.WebApi
{
    public class DataResult<TData> : BaseResult<BaseMeta>
    {
        public DataResult(TData? data = default)
            : base(true, 200)
        {
            Data = data;
        }

        [JsonProperty(Order = -9)]
        [DisplayName("kết quả xử lý")]
        public virtual TData? Data { get; set; }
    }
}
