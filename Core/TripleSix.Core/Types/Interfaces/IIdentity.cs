using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace TripleSix.Core.Types
{
    /// <summary>
    /// Định danh người thao tác.
    /// </summary>
    public interface IIdentity
    {
        /// <summary>
        /// HTTP context phiên xủ lý.
        /// </summary>
        HttpContext? HttpContext { get; }

        /// <summary>
        /// Thông tin người thao tác.
        /// </summary>
        ClaimsPrincipal? User { get; }

        /// <summary>
        /// Id người thao tác.
        /// </summary>
        Guid? UserId { get; }
    }
}
