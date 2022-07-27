using System.Diagnostics;
using System.Runtime.CompilerServices;
using AutoMapper;
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
        /// Automapper.
        /// </summary>
        public IMapper Mapper { get; set; }

        /// <summary>
        /// Http context accessor.
        /// </summary>
        public IHttpContextAccessor? HttpContextAccessor { get; set; }

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
