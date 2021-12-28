using Newtonsoft.Json;

namespace TripleSix.Core.WebApi.Results
{
    public class SuccessResult : BaseResult<BaseMeta>
    {
        public SuccessResult()
            : base(200)
        {
        }

        [JsonProperty(Order = -9)]
        public virtual bool Data { get; protected set; } = true;
    }
}