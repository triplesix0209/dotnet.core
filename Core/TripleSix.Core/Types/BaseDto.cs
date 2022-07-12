using Microsoft.AspNetCore.Http;

namespace TripleSix.Core.Types
{
    /// <summary>
    /// DTO cơ bản.
    /// </summary>
    public abstract class BaseDto : IDto
    {
        private HttpContext? _httpContext = null;

        /// <inheritdoc/>
        public void SetHttpContext(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        /// <inheritdoc/>
        public HttpContext? GetHttpContext()
        {
            return _httpContext;
        }
    }
}
