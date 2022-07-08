using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TripleSix.Core.Services.Interfaces;

namespace TripleSix.Core.Services
{
    /// <summary>
    /// Service cơ bản.
    /// </summary>
    public abstract class BaseService : IService
    {
        /// <summary>
        /// Represents a set of key/value application configuration properties.
        /// </summary>
        public IConfiguration Configuration { get; set; }

        /// <summary>
        /// Automapper.
        /// </summary>
        public IMapper Mapper { get; set; }

        /// <summary>
        /// Provides access to the current HttpContext.
        /// </summary>
        public IHttpContextAccessor HttpContextAccessor { get; set; }
    }
}
