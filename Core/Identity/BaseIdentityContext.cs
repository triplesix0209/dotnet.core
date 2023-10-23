using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TripleSix.Core.Appsettings;

namespace TripleSix.Core.Identity
{
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

            try
            {
                var claims = ReadToken(httpContext, configuration);
                ParseData(claims);
            }
            catch
            {
                Id = null;
            }
        }

        // Id tài khoản
        public Guid? Id { get; set; }

        /// <summary>
        /// Đọc và kiểm tra Token.
        /// </summary>
        /// <param name="httpContext"><see cref="HttpContext"/>.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        /// <returns>Danh sách các <see cref="Claim"/>.</returns>
        protected virtual IEnumerable<Claim> ReadToken(HttpContext httpContext, IConfiguration configuration)
        {
            var accessToken = httpContext.Request.Headers.Authorization.First();
            if (accessToken == null) throw new NullReferenceException(nameof(accessToken));
            if (accessToken.Split(" ").Length > 1) accessToken = accessToken.Split(" ")[1];

            var appsetting = new IdentityAppsetting(configuration);
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appsetting.SigningKey));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = appsetting.ValidateIssuer,
                ValidIssuer = appsetting.ValidIssuer,
            };

            var claimPrincipal = new JwtSecurityTokenHandler()
                .ValidateToken(accessToken, tokenValidationParameters, out _);
            return claimPrincipal.Claims;
        }

        /// <summary>
        /// Đọc và nhận thông tin của Token.
        /// </summary>
        /// <param name="claims">Danh sách các <see cref="Claim"/>.</param>
        protected abstract void ParseData(IEnumerable<Claim> claims);
    }
}
