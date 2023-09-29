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
                var accessToken = httpContext.Request.Headers.Authorization.First();
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
                LoadDataFromToken(jwtToken);
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

        protected abstract void LoadDataFromToken(JwtSecurityToken token);
    }
}
