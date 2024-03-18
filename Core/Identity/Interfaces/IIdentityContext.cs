using Microsoft.AspNetCore.Http;

namespace TripleSix.Core.Identity
{
    /// <summary>
    /// Identity context.
    /// </summary>
    public interface IIdentityContext
    {
        /// <summary>
        /// Id người thao tác.
        /// </summary>
        Guid? Id { get; set; }

        /// <summary>
        /// Danh sách quyền hạn.
        /// </summary>
        IEnumerable<string> Scope { get; set; }

        /// <summary>
        /// Issuer (nơi tạo và ký token).
        /// </summary>
        string? Issuer { get; set; }

        /// <summary>
        /// Lấy Access Token.
        /// </summary>
        /// <param name="httpContext"><see cref="HttpContext"/>.</param>
        /// <returns>Access Token.</returns>
        string? GetAccessToken(HttpContext httpContext);
    }
}
