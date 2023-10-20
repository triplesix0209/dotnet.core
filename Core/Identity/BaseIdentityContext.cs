using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace TripleSix.Core.Identity
{
    public abstract class BaseIdentityContext : IIdentityContext
    {
        /// <summary>
        /// Khởi tạo <see cref="BaseIdentityContext"/>.
        /// </summary>
        /// <param name="httpContext"><see cref="HttpContext"/>.</param>
        protected BaseIdentityContext(HttpContext? httpContext)
        {
            if (httpContext == null)
            {
                IsVaild = false;
                return;
            }

            try
            {
                var token = GetToken(httpContext);
                ParseData(token);
                IsVaild = true;
            }
            catch
            {
                IsVaild = false;
            }
        }

        // Phiên xác thực hợp lệ
        public bool IsVaild { get; }

        // Id tài khoản
        public Guid Id { get; set; }

        protected virtual JwtSecurityToken GetToken(HttpContext httpContext)
        {
            var accessToken = httpContext.Request.Headers.Authorization.First();
            if (accessToken == null) throw new NullReferenceException(nameof(accessToken));
            if (accessToken.Split(" ").Length > 1) accessToken = accessToken.Split(" ")[1];

            return new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
        }

        protected abstract void ParseData(JwtSecurityToken token);
    }
}
