using Microsoft.AspNetCore.Http;

namespace TripleSix.Core.Services
{
    /// <summary>
    /// Service cơ bản.
    /// </summary>
    public abstract class BaseService
        : IService
    {
        /// <summary>
        /// Provides access to the current HttpContext.
        /// </summary>
        public IHttpContextAccessor? HttpContextAccessor { get; set; }
    }
}
