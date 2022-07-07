using AutoMapper;
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
        /// Configuration từ appsetting.
        /// </summary>
        public IConfiguration? Configuration { get; set; }

        /// <summary>
        /// Automapper.
        /// </summary>
        public IMapper? Mapper { get; set; }
    }
}
