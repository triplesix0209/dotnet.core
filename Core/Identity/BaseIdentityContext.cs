#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TripleSix.Core.Appsettings;
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
                var claims = ValidateAccessToken(accessToken, configuration);
                ParseData(claims);
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
        public virtual string? GetAccessToken(HttpContext httpContext)
        {
            var authorizationValue = httpContext.Request.Headers.Authorization.FirstOrDefault();
            if (authorizationValue == null) return null;
            return authorizationValue.Split(' ')[^1];
        }

        /// <inheritdoc/>
        public virtual ClaimsPrincipal ValidateAccessToken(string accessToken, IdentityAppsetting setting)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(setting.SigningKey));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = setting.ValidateIssuer,
                ValidIssuers = setting.Issuer,
                ValidateAudience = setting.ValidateAudience,
                ValidAudiences = setting.Audience,
            };

            return new JwtSecurityTokenHandler().ValidateToken(accessToken, tokenValidationParameters, out _);
        }

        /// <inheritdoc/>
        public virtual ClaimsPrincipal ValidateAccessToken(string accessToken, IConfiguration configuration)
        {
            return ValidateAccessToken(accessToken, new IdentityAppsetting(configuration));
        }

        /// <summary>
        /// Đọc và nhận thông tin của Token.
        /// </summary>
        /// <param name="claims">Danh sách các <see cref="Claim"/>.</param>
        protected virtual void ParseData(ClaimsPrincipal claims)
        {
            Id = Guid.Parse(claims.FindFirstValue(nameof(Id).ToCamelCase())!);
            Scope = claims.FindFirstValue(nameof(Scope).ToCamelCase())!.Split(' ');
        }
    }
}
