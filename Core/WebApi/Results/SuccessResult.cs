using System.ComponentModel;
using Newtonsoft.Json;

namespace TripleSix.Core.WebApi
{
    public class SuccessResult : BaseResult<BaseMeta>
    {
        public SuccessResult()
            : base(true, 200)
        {
        }

        public SuccessResult(int httpStatusCode)
            : base(true, httpStatusCode)
        {
        }

        [JsonProperty(Order = -9)]
        [DisplayName("kết quả xử lý")]
        public virtual bool Data => true;
    }
}
