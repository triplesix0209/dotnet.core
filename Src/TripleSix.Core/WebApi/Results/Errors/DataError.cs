using Newtonsoft.Json;

namespace TripleSix.Core.WebApi.Results
{
    public class DataError<TData> : BaseError
    {
        [JsonProperty(Order = -9)]
        public TData Data { get; set; }
    }
}