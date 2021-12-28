using CpTech.Core.Extensions;
using Newtonsoft.Json;

namespace CpTech.Core.WebApi.Results
{
    public class BaseError
        : IError
    {
        private string _code;

        [JsonProperty(Order = -10)]
        public virtual string Code
        {
            get => _code;
            set => _code = value.ToSnakeCase();
        }

        [JsonProperty(Order = -10)]
        public virtual string Message { get; set; }
    }
}