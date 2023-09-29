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
            try
            {
                var accessToken = httpContext!.Request.Headers.Authorization.First();

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(accessToken);
                LoadDataFromToken(jwtToken);
                IsVaild = true;
            }
            catch
            {
                IsVaild = false;
            }
        }

        public bool IsVaild { get; }

        public Guid Id { get; set; }

        protected abstract void LoadDataFromToken(JwtSecurityToken token);
    }
}
