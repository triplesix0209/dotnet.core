using Microsoft.AspNetCore.Http;

namespace TripleSix.Core.Types
{
    /// <summary>
    /// DTO cơ bản.
    /// </summary>
    public interface IDto
    {
        /// <summary>
        /// Set Http Context.
        /// </summary>
        /// <param name="httpContext">Encapsulates all HTTP-specific information about an individual HTTP request.</param>
        void SetHttpContext(HttpContext httpContext);

        /// <summary>
        /// Get Http Context.
        /// </summary>
        /// <returns>Encapsulates all HTTP-specific information about an individual HTTP request.</returns>
        HttpContext? GetHttpContext();
    }
}
