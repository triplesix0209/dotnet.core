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
        /// Http context accessor.
        /// </summary>
        public IHttpContextAccessor? HttpContextAccessor { get; set; }
    }
}
