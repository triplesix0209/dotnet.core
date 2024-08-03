using System.Diagnostics;
using System.Runtime.CompilerServices;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TripleSix.Core.Services
{
    /// <summary>
    /// Service cơ bản.
    /// </summary>
    public abstract class BaseService
        : IService
    {
        /// <summary>
        /// <see cref="IConfiguration"/>.
        /// </summary>
        public IConfiguration Configuration { get; set; }

        /// <summary>
        /// <see cref="IMapper"/>.
        /// </summary>
        public IMapper Mapper { get; set; }

        /// <summary>
        /// <see cref="IHttpContextAccessor"/>.
        /// </summary>
        public IHttpContextAccessor? HttpContextAccessor { get; set; }

        /// <summary>
        /// <see cref="ILogger"/>.
        /// </summary>
        public ILogger<IService> Logger { get; set; }

        /// <summary>
        /// Service Provider.
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// Tạo activity để trace cho method hiện tại.
        /// </summary>
        /// <param name="callerName">Tên hàm đang xử lý.</param>
        /// <returns><see cref="Activity"/>.</returns>
        internal static Activity? StartTraceMethodActivity([CallerMemberName] string callerName = "")
        {
            return Activity.Current?.Source.StartActivity($"Core.{callerName}");
        }
    }
}
