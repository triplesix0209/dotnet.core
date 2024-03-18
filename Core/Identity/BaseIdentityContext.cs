#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Identity
{
    /// <summary>
    /// Base identity.
    /// </summary>
    public abstract class BaseIdentityContext : IIdentityContext
    {
        /// <summary>
        /// Khởi tạo <see cref="BaseIdentityContext"/>.
        /// </summary>
        /// <param name="httpContext"><see cref="HttpContext"/>.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        protected BaseIdentityContext(HttpContext? httpContext, IConfiguration configuration)
        {
            if (httpContext == null) return;

            var accessToken = GetAccessToken(httpContext);
            if (accessToken == null) return;

            try
            {
                var data = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
                ParseData(data.Claims);
            }
            catch
            {
                Id = null;
            }
        }

        /// <inheritdoc/>
        public Guid? Id { get; set; }

        /// <inheritdoc/>
        public IEnumerable<string> Scope { get; set; }

        /// <inheritdoc/>
        public string? Issuer { get; set; }

        /// <inheritdoc/>
        public virtual string? GetAccessToken(HttpContext httpContext)
        {
            var authorizationValue = httpContext.Request.Headers.Authorization.FirstOrDefault();
            if (authorizationValue == null) return null;
            return authorizationValue.Split(' ')[^1];
        }

        /// <summary>
        /// Đọc và nhận thông tin của Token.
        /// </summary>
        /// <param name="claims">Danh sách các <see cref="Claim"/>.</param>
        protected virtual void ParseData(IEnumerable<Claim> claims)
        {
            Id = Guid.Parse(claims.FindFirstValue(nameof(Id).ToCamelCase())!);
            Scope = claims.FindFirstValue(nameof(Scope).ToCamelCase())!.Split(' ');
            Issuer = claims.FindFirstValue("iss");
        }
    }
}
