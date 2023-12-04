using System.ComponentModel;
using Newtonsoft.Json;

namespace TripleSix.CoreOld.WebApi.Results
{
    public class SuccessResult : BaseResult<SuccessMeta>
    {
        public SuccessResult()
            : base(200)
        {
        }

        [JsonProperty(Order = -9)]
        [DisplayName("kết quả xử lý")]
        public virtual bool Data { get; protected set; } = true;
    }
}