using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CpTech.Core.WebApi.Results
{
    public abstract class BaseResult<TMeta>
        : IActionResult
        where TMeta : IMeta, new()
    {
        protected BaseResult(int statusCode)
        {
            StatusCode = statusCode;
            Meta = new TMeta { Success = statusCode >= 200 && statusCode < 300 };
        }

        [JsonIgnore]
        public virtual int StatusCode { get; protected set; }

        [JsonProperty(Order = -10)]
        public virtual TMeta Meta { get; protected set; }

        public virtual async Task ExecuteResultAsync(ActionContext context)
        {
            var result = new ObjectResult(this) { StatusCode = StatusCode };
            await result.ExecuteResultAsync(context);
        }
    }
}