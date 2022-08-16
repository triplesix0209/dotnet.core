using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using TripleSix.Core.Appsettings;

namespace TripleSix.Core.Helpers
{
    /// <summary>
    /// Helper xử lý JWT.
    /// </summary>
    public static class JwtHelper
    {
        /// <summary>
        /// Tạo JWT token từ danh sách claim.
        /// </summary>
        /// <param name="claims">Danh sách claim dùng để xử lý.</param>
        /// <param name="appsetting"><see cref="IdentityAppsetting"/>.</param>
        /// <returns>JWT Token.</returns>
        public static string GenerateJwtToken(IEnumerable<Claim> claims, IdentityAppsetting appsetting)
        {
            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(appsetting.AccessTokenLifetime),
                issuer: appsetting.Issuer,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appsetting.SecretKey)),
                    SecurityAlgorithms.HmacSha256)));
        }

        /// <summary>
        /// Validate JWT token.
        /// </summary>
        /// <param name="token">Token cần kiểm tra.</param>
        /// <param name="appsetting"><see cref="IdentityAppsetting"/>.</param>
        /// <returns><see cref="TokenValidationResult"/>.</returns>
        public static TokenValidationResult ValidateJwtToken(string token, IdentityAppsetting appsetting)
        {
            return new JsonWebTokenHandler().ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appsetting.SecretKey)),

                ValidateIssuer = true,
                ValidIssuer = appsetting.Issuer,

                ValidateAudience = false,

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            });
        }

        /// <summary>
        /// Đọc dữ liệu JWT token.
        /// </summary>
        /// <param name="token">Token cần đọc.</param>
        /// <returns><see cref="JwtSecurityToken"/>.</returns>
        public static JwtSecurityToken? ReadJwtToken(string token)
        {
            return new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
        }
    }
}