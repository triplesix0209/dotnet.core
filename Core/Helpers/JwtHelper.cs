using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

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
        /// <param name="secretKey">Secret Key dùng để mã hóa.</param>
        /// <param name="accessTokenLifetime">Thời gian tồn tại của token (phút).</param>
        /// <param name="issuer">Token issuer.</param>
        /// <returns>JWT Token.</returns>
        public static string GenerateJwtToken(IEnumerable<Claim> claims, string secretKey, int accessTokenLifetime, string? issuer)
        {
            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(accessTokenLifetime),
                issuer: issuer,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    SecurityAlgorithms.HmacSha256)));
        }

        /// <summary>
        /// Validate JWT token.
        /// </summary>
        /// <param name="token">Token cần kiểm tra.</param>
        /// <param name="secretKey">Secret Key dùng để mã hóa.</param>
        /// <param name="issuer">Token issuer.</param>
        /// <returns><see cref="TokenValidationResult"/>.</returns>
        public static TokenValidationResult ValidateJwtToken(string token, string secretKey, string? issuer)
        {
            return new JsonWebTokenHandler().ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),

                ValidateIssuer = issuer != null,
                ValidIssuer = issuer,

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