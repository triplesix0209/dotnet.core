using Newtonsoft.Json;

namespace CpTech.Core.WebApi.Results
{
    public class DataError<TData> : BaseError
    {
        [JsonProperty(Order = -9)]
        public TData Data { get; set; }
    }
}