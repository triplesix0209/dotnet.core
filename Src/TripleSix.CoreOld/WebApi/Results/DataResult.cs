using System.ComponentModel;
using Newtonsoft.Json;

namespace TripleSix.CoreOld.WebApi.Results
{
    public class DataResult<TData, TMeta> : BaseResult<TMeta>
        where TMeta : IMeta, new()
    {
        public DataResult(TData data = default)
            : base(200)
        {
            Data = data;
        }

        [JsonProperty(Order = -9)]
        [DisplayName("kết quả xử lý")]
        public virtual TData Data { get; protected set; }
    }
}