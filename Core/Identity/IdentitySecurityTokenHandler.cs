using System.Collections.Concurrent;
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
        private static readonly ConcurrentDictionary<string, SigningKeyCacheItem> _signingKeyCaches = new();
        private static readonly HttpClient _jwksHttpClient = new(new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(5),
        })
        {
            Timeout = TimeSpan.FromSeconds(10),
        };

        private static readonly SemaphoreSlim _jwksFetchLock = new(1, 1);

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
        public override async Task<TokenValidationResult> ValidateTokenAsync(string token, TokenValidationParameters validationParameters)
        {
            // nạp trước JWKS bằng HTTP async, để ValidateToken sync bên dưới đọc cache mà không block thread chờ HTTP
            if (Setting.SigningKeyMode == IdentitySigningKeyModes.Jwks)
            {
                if (Setting.JwksEndpoint.IsNullOrEmpty())
                    throw new ArgumentNullException(nameof(Setting.JwksEndpoint));

                var tokenData = ReadJwtToken(token);
                var jwksCacheKey = GetJwksCacheKey(tokenData);
                if (GetValidCacheItem(jwksCacheKey) == null)
                {
                    // khóa dedupe: khi cache hết hạn, chỉ 1 request fetch JWKS, các request khác chờ dùng lại cache
                    await _jwksFetchLock.WaitAsync();
                    try
                    {
                        if (GetValidCacheItem(jwksCacheKey) == null)
                        {
                            var jwksResponse = await _jwksHttpClient.GetStringAsync(Setting.JwksEndpoint);
                            CacheJwksSigningKey(tokenData, jwksCacheKey, jwksResponse);
                        }
                    }
                    finally
                    {
                        _jwksFetchLock.Release();
                    }
                }
            }

            return await base.ValidateTokenAsync(token, validationParameters);
        }

        /// <inheritdoc/>
        public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            var tokenData = ReadJwtToken(token);
            var issuer = tokenData.Issuer;
            var cacheKey = $"{Setting.SigningKeyMode}_{issuer}";

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

                    var dynamicCacheItem = GetValidCacheItem(cacheKey);
                    if (dynamicCacheItem == null)
                    {
                        signingKey = GetSigningKeyMethod(Setting, tokenData);
                        if (signingKey.IsNullOrEmpty()) throw new ArgumentNullException(nameof(signingKey));

                        var expiredAt = DateTime.UtcNow.AddSeconds(Setting.SigningKeyCacheTimelife);
                        dynamicCacheItem = new SigningKeyCacheItem(signingKey, expiredAt);
                        _signingKeyCaches[cacheKey] = dynamicCacheItem;
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

                    var jwksCacheKey = GetJwksCacheKey(tokenData);
                    var jwksCacheItem = GetValidCacheItem(jwksCacheKey);
                    if (jwksCacheItem == null)
                    {
                        // fallback cho đường gọi sync (ISecurityTokenValidator không có bản async);
                        // request qua JwtBearer đi vào ValidateTokenAsync nên cache đã được nạp bằng HTTP async trước đó
                        var jwksResponse = _jwksHttpClient.GetStringAsync(Setting.JwksEndpoint).GetAwaiter().GetResult();
                        jwksCacheItem = CacheJwksSigningKey(tokenData, jwksCacheKey, jwksResponse);
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
                case "HS256":
                    validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
                    break;

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

                default:
                    throw new NotSupportedException($"Algorithm {algorithm} không được hỗ trợ.");
            }

            return base.ValidateToken(token, validationParameters, out validatedToken);
        }

        private static SigningKeyCacheItem? GetValidCacheItem(string cacheKey)
        {
            return _signingKeyCaches.TryGetValue(cacheKey, out var item) && DateTime.UtcNow <= item.ExpiredAt
                ? item
                : null;
        }

        private string GetJwksCacheKey(JwtSecurityToken tokenData)
        {
            var tokenKid = tokenData.Header.Kid;
            return $"{Setting.SigningKeyMode}_{tokenData.Issuer}_{(tokenKid.IsNullOrEmpty() ? "default" : tokenKid)}";
        }

        private SigningKeyCacheItem CacheJwksSigningKey(JwtSecurityToken tokenData, string jwksCacheKey, string? jwksResponse)
        {
            if (jwksResponse.IsNullOrEmpty()) throw new ArgumentNullException("jwks response");

            var jwks = new JsonWebKeySet(jwksResponse);
            var tokenKid = tokenData.Header.Kid;

            var jwkItem = (!tokenKid.IsNullOrEmpty() ? jwks.Keys.FirstOrDefault(k => k.Kid == tokenKid) : null)
                ?? jwks.Keys.FirstOrDefault(k => k.Kid == tokenData.Issuer)
                ?? throw new ArgumentNullException("jwk item");

            var jwk = Newtonsoft.Json.JsonConvert.SerializeObject(jwkItem);
            var expiredAt = DateTime.UtcNow.AddSeconds(Setting.SigningKeyCacheTimelife);
            var jwksCacheItem = new SigningKeyCacheItem(jwk, expiredAt);
            _signingKeyCaches[jwksCacheKey] = jwksCacheItem;

            return jwksCacheItem;
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
