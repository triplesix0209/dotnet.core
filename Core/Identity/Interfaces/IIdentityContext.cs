using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TripleSix.Core.Appsettings;

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
        public Guid? Id { get; set; }

        /// <summary>
        /// Danh sách quyền hạn.
        /// </summary>
        public IEnumerable<string> Scope { get; set; }

        /// <summary>
        /// Lấy Access Token.
        /// </summary>
        /// <param name="httpContext"><see cref="HttpContext"/>.</param>
        /// <returns>Access Token.</returns>
        string? GetAccessToken(HttpContext httpContext);

        /// <summary>
        /// Lấy Access Token.
        /// </summary>
        /// <param name="accessToken">Access Token.</param>
        /// <param name="setting"><see cref="IdentityAppsetting"/>.</param>
        /// <returns><see cref="ClaimsPrincipal"/>.</returns>
        ClaimsPrincipal ValidateAccessToken(string accessToken, IdentityAppsetting setting);

        /// <summary>
        /// Lấy Access Token.
        /// </summary>
        /// <param name="accessToken">Access Token.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        /// <returns><see cref="ClaimsPrincipal"/>.</returns>
        ClaimsPrincipal ValidateAccessToken(string accessToken, IConfiguration configuration);
    }
}
