using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace TripleSix.Core.Identity
{
    public interface IIdentityContext
    {
        /// <summary>
        /// Id người thao tác.
        /// </summary>
        public Guid? Id { get; set; }

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
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        /// <returns>Danh sách các <see cref="Claim"/>.</returns>
        IEnumerable<Claim> ValidateAccessToken(string accessToken, IConfiguration configuration);
    }
}
