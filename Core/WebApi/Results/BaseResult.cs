﻿using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TripleSix.Core.WebApi
{
    public abstract class BaseResult<TMeta>
        : IActionResult
        where TMeta : BaseMeta, new()
    {
        protected BaseResult(bool success, int httpStatusCode)
        {
            HttpStatusCode = httpStatusCode;
            Meta = new TMeta { Success = success };
        }

        [JsonIgnore]
        [DisplayName("Http status code")]
        public virtual int HttpStatusCode { get; set; }

        [JsonProperty(Order = -10)]
        [DisplayName("thông tin metadata")]
        public virtual TMeta Meta { get; set; }

        /// <inheritdoc/>
        public async Task ExecuteResultAsync(ActionContext context)
        {
            var result = new ObjectResult(this) { StatusCode = HttpStatusCode };
            await result.ExecuteResultAsync(context);
        }
    }
}
