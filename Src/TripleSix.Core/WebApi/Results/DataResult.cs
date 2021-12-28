using Newtonsoft.Json;

namespace TripleSix.Core.WebApi.Results
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
        public virtual TData Data { get; protected set; }
    }
}