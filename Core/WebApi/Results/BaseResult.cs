using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Base result.
    /// </summary>
    /// <typeparam name="TMeta">Loại metadata.</typeparam>
    public abstract class BaseResult<TMeta>
        : IActionResult
        where TMeta : BaseMeta, new()
    {
        /// <summary>
        /// Base result.
        /// </summary>
        /// <param name="success">Có xử lý thành công?.</param>
        /// <param name="httpStatusCode">Mã trạng thái http.</param>
        protected BaseResult(bool success, int httpStatusCode)
        {
            HttpStatusCode = httpStatusCode;
            Meta = new TMeta { Success = success };
        }

        /// <summary>
        /// Mã trạng thái http.
        /// </summary>
        [JsonIgnore]
        [DisplayName("Mã trạng thái http")]
        public virtual int HttpStatusCode { get; set; }

        /// <summary>
        /// Metadata.
        /// </summary>
        [JsonProperty(Order = -10)]
        [DisplayName("Metadata")]
        public virtual TMeta Meta { get; set; }

        /// <inheritdoc/>
        public async Task ExecuteResultAsync(ActionContext context)
        {
            var result = new ObjectResult(this) { StatusCode = HttpStatusCode };
            await result.ExecuteResultAsync(context);
        }
    }
}
