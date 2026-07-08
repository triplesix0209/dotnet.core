using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TripleSix.Core.Appsettings;
using TripleSix.Core.Constants;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Identity
{
    /// <summary>
    /// Dynamic Key JWT Token Handler.
    /// </summary>
    public class IdentitySecurityTokenHandler : JwtSecurityTokenHandler,
        ISecurityTokenValidator
    {
        private static Dictionary<string, SigningKeyCacheItem> _signingKeyCaches = new();

        /// <summary>
        /// Create instance of <see cref="IdentitySecurityTokenHandler"/>.
        /// </summary>
        /// <param name="setting"><see cref="IdentityAppsetting"/>.</param>
        public IdentitySecurityTokenHandler(IdentityAppsetting setting)
        {
            Setting = setting;
            GetSigningKeyMethod = null;
        }

        /// <summary>
        /// <see cref="IdentityAppsetting"/>.
        /// </summary>
        public IdentityAppsetting Setting { get; }

        /// <summary>
        /// Hàm lấy signing key.
        /// </summary>
        public Func<IdentityAppsetting, JwtSecurityToken, string?>? GetSigningKeyMethod { get; set; }

        /// <inheritdoc/>
        public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            var tokenData = ReadJwtToken(token);
            var issuer = tokenData.Issuer;

            if (Setting.BypassUserIds.IsNotNullOrEmpty())
            {
                var userId = tokenData.Claims.FindFirstValue(nameof(IIdentityContext.Id).ToCamelCase());
                if (userId.IsNotNullOrEmpty() && Setting.BypassUserIds.Contains(userId))
                {
                    validationParameters.ValidateLifetime = false;
                }
            }

            string? signingKey;
            switch (Setting.SigningKeyMode)
            {
                case IdentitySigningKeyModes.Dynamic:
                    if (GetSigningKeyMethod == null)
                        throw new ArgumentNullException(nameof(GetSigningKeyMethod));

                    var dynamicCacheItem = _signingKeyCaches.ContainsKey(issuer) ? _signingKeyCaches[issuer] : null;
                    if (dynamicCacheItem == null || DateTime.UtcNow > dynamicCacheItem.ExpiredAt)
                    {
                        signingKey = GetSigningKeyMethod(Setting, tokenData);
                        if (signingKey.IsNullOrEmpty()) throw new ArgumentNullException(nameof(signingKey));

                        var expiredAt = DateTime.UtcNow.AddSeconds(Setting.SigningKeyCacheTimelife);
                        dynamicCacheItem = new SigningKeyCacheItem(signingKey, expiredAt);
                        _signingKeyCaches[issuer] = dynamicCacheItem;
                    }

                    signingKey = dynamicCacheItem.SigningKey;
                    break;

                case IdentitySigningKeyModes.Static:
                    signingKey = Setting.IssuerSigningKey.FirstOrDefault(x => x.Issuer == issuer)?.SigningKey;
                    if (signingKey.IsNullOrEmpty())
                        throw new ArgumentNullException(nameof(Setting.IssuerSigningKey));
                    break;

                case IdentitySigningKeyModes.Jwks:
                    if (Setting.JwksEndpoint.IsNullOrEmpty())
                        throw new ArgumentNullException(nameof(Setting.JwksEndpoint));

                    var jwksCacheItem = _signingKeyCaches.ContainsKey(issuer) ? _signingKeyCaches[issuer] : null;
                    if (jwksCacheItem == null || DateTime.UtcNow > jwksCacheItem.ExpiredAt)
                    {
                        using var httpClient = new HttpClient();
                        var jwksResponse = httpClient.GetStringAsync(Setting.JwksEndpoint).GetAwaiter().GetResult();
                        if (jwksResponse.IsNullOrEmpty()) throw new ArgumentNullException("jwks response");

                        var jwks = new JsonWebKeySet(jwksResponse);
                        var jwkItem = jwks.Keys.FirstOrDefault(k => k.Kid == issuer)
                                  ?? throw new ArgumentNullException("jwk item");

                        var jwk = Newtonsoft.Json.JsonConvert.SerializeObject(jwkItem);
                        var expiredAt = DateTime.UtcNow.AddSeconds(Setting.SigningKeyCacheTimelife);
                        jwksCacheItem = new SigningKeyCacheItem(jwk, expiredAt);
                        _signingKeyCaches[issuer] = jwksCacheItem;
                    }

                    signingKey = jwksCacheItem.SigningKey;
                    break;

                default:
                    throw new NotSupportedException($"SigningKeyMode {Setting.SigningKeyMode} không được hỗ trợ.");
            }

            var algorithm = tokenData.Header.Alg;
            if (algorithm.IsNullOrEmpty()) algorithm = Setting.Algorithm;
            switch (algorithm)
            {
                case "ES256":
                    if (signingKey.Contains("\"kty\""))
                    {
                        validationParameters.IssuerSigningKey = new JsonWebKey(signingKey);
                    }
                    else
                    {
                        var ecdsa = ECDsa.Create();
                        ecdsa.ImportFromPem(signingKey);
                        validationParameters.IssuerSigningKey = new ECDsaSecurityKey(ecdsa);
                    }

                    break;

                case "HS256":
                    validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
                    break;

                default:
                    throw new NotSupportedException($"Algorithm {alg} không được hỗ trợ.");
            }

            return base.ValidateToken(token, validationParameters, out validatedToken);
        }

        private class SigningKeyCacheItem
        {
            public SigningKeyCacheItem(string signingKey, DateTime expiredAt)
            {
                SigningKey = signingKey;
                ExpiredAt = expiredAt;
            }

            public string SigningKey { get; set; }

            public DateTime ExpiredAt { get; set; }
        }
    }
}
