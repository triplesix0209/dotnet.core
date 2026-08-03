using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using RestSharp;
using TripleSix.Core.Appsettings;

namespace TripleSix.Core.Helpers
{
    /// <summary>
    /// Helper sinh và quản lý SignKey cho ApiKey.
    /// </summary>
    public static class ApiKeyHelper
    {
        /// <summary>
        /// Sinh chuỗi SignKey (JWT token ES256) bằng Private Key.
        /// </summary>
        /// <param name="appCode">Mã App (AppCode / Issuer).</param>
        /// <param name="privateKeyPem">Private Key dạng PEM.</param>
        /// <param name="lifetimeSeconds">Thời gian sống của SignKey (giây).</param>
        /// <param name="payload">Payload bổ sung nếu có.</param>
        /// <returns>Chuỗi SignKey.</returns>
        public static string GenerateSignKey(
            string appCode,
            string privateKeyPem,
            int lifetimeSeconds = 300,
            string? payload = null)
        {
            if (appCode.IsNullOrEmpty()) throw new ArgumentNullException(nameof(appCode));
            if (privateKeyPem.IsNullOrEmpty()) throw new ArgumentNullException(nameof(privateKeyPem));

            using var ecdsa = ECDsa.Create();
            ecdsa.ImportFromPem(privateKeyPem);

            var keyId = appCode.StartsWith("api-key-") ? appCode : $"api-key-{appCode}";
            var credentials = new SigningCredentials(
                new ECDsaSecurityKey(ecdsa) { KeyId = keyId },
                SecurityAlgorithms.EcdsaSha256);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iss, appCode),
            };

            if (payload.IsNotNullOrEmpty())
            {
                claims.Add(new Claim("payload", payload!));
            }

            var now = DateTime.UtcNow;
            var token = new JwtSecurityToken(
                issuer: appCode,
                claims: claims,
                notBefore: now.AddSeconds(-30),
                expires: now.AddSeconds(lifetimeSeconds),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Sinh chuỗi SignKey (JWT token ES256) từ <see cref="ApiKeyAppsetting"/>.
        /// </summary>
        /// <param name="setting"><see cref="ApiKeyAppsetting"/>.</param>
        /// <param name="payload">Payload bổ sung nếu có.</param>
        /// <returns>Chuỗi SignKey.</returns>
        public static string GenerateSignKey(ApiKeyAppsetting setting, string? payload = null)
        {
            return GenerateSignKey(
                setting.AppCode,
                setting.PrivateKey,
                setting.SignKeyLifetimeSeconds,
                payload);
        }

        /// <summary>
        /// Thêm header chứa SignKey vào RestRequest.
        /// </summary>
        /// <param name="request"><see cref="RestRequest"/>.</param>
        /// <param name="setting"><see cref="ApiKeyAppsetting"/>.</param>
        /// <param name="payload">Payload bổ sung nếu có.</param>
        /// <param name="headerName">Tên header, mặc định là X-Sign.</param>
        /// <returns><see cref="RestRequest"/>.</returns>
        public static RestRequest AddHeaderApiSignKey(
            this RestRequest request,
            ApiKeyAppsetting setting,
            string? payload = null,
            string headerName = "X-Sign")
        {
            var signKey = GenerateSignKey(setting, payload);
            request.AddHeader(headerName, signKey);
            return request;
        }

        /// <summary>
        /// Thêm header chứa SignKey vào RestRequest.
        /// </summary>
        /// <param name="request"><see cref="RestRequest"/>.</param>
        /// <param name="appCode">Mã App (AppCode / Issuer).</param>
        /// <param name="privateKeyPem">Private Key dạng PEM.</param>
        /// <param name="lifetimeSeconds">Thời gian sống của SignKey (giây).</param>
        /// <param name="payload">Payload bổ sung nếu có.</param>
        /// <param name="headerName">Tên header, mặc định là X-Sign.</param>
        /// <returns><see cref="RestRequest"/>.</returns>
        public static RestRequest AddHeaderApiSignKey(
            this RestRequest request,
            string appCode,
            string privateKeyPem,
            int lifetimeSeconds = 300,
            string? payload = null,
            string headerName = "X-Sign")
        {
            var signKey = GenerateSignKey(appCode, privateKeyPem, lifetimeSeconds, payload);
            request.AddHeader(headerName, signKey);
            return request;
        }
    }
}
