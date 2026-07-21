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
    /// <remarks>
    /// Create instance of <see cref="IdentitySecurityTokenHandler"/>.
    /// </remarks>
    /// <param name="setting"><see cref="IdentityAppsetting"/>.</param>
    public class IdentitySecurityTokenHandler(IdentityAppsetting setting) : JwtSecurityTokenHandler,
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
        /// <see cref="IdentityAppsetting"/>.
        /// </summary>
        public IdentityAppsetting Setting { get; } = setting;

        /// <summary>
        /// Hàm lấy signing key.
        /// </summary>
        public Func<IdentityAppsetting, JwtSecurityToken, string?>? GetSigningKeyMethod { get; set; } = null;

        /// <inheritdoc/>
        public override async Task<TokenValidationResult> ValidateTokenAsync(string token, TokenValidationParameters validationParameters)
        {
            var clonedParameters = validationParameters.Clone();
            await PrepareValidationParametersAsync(token, clonedParameters, isAsync: true);
            return await base.ValidateTokenAsync(token, clonedParameters);
        }

        /// <inheritdoc/>
        public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            var clonedParameters = validationParameters.Clone();
            PrepareValidationParametersAsync(token, clonedParameters, isAsync: false).GetAwaiter().GetResult();
            return base.ValidateToken(token, clonedParameters, out validatedToken);
        }

        private static SigningKeyCacheItem? GetValidCacheItem(string cacheKey)
        {
            return _signingKeyCaches.TryGetValue(cacheKey, out var item) && DateTime.UtcNow <= item.ExpiredAt
                ? item
                : null;
        }

        private async Task PrepareValidationParametersAsync(string token, TokenValidationParameters validationParameters, bool isAsync)
        {
            var tokenData = ReadJwtToken(token);

            if (Setting.BypassUserIds.IsNotNullOrEmpty())
            {
                var userId = tokenData.Claims.FindFirstValue(nameof(IIdentityContext.Id).ToCamelCase());
                if (userId.IsNotNullOrEmpty() && Setting.BypassUserIds.Contains(userId))
                {
                    validationParameters.ValidateLifetime = false;
                }
            }

            var issuer = tokenData.Issuer;
            var cacheKey = tokenData.Header.Kid.IsNotNullOrEmpty() ? tokenData.Header.Kid : issuer;
            var cacheItem = GetValidCacheItem(cacheKey);

            string? signingKey;
            switch (Setting.SigningKeyMode)
            {
                case IdentitySigningKeyModes.Dynamic:
                    if (GetSigningKeyMethod == null)
                        throw new Exception($"{nameof(GetSigningKeyMethod)} is not set");

                    if (cacheItem == null)
                    {
                        signingKey = GetSigningKeyMethod(Setting, tokenData);
                        if (signingKey.IsNullOrEmpty()) throw new Exception($"{nameof(signingKey)} is null or empty");

                        var expiredAt = DateTime.UtcNow.AddSeconds(Setting.SigningKeyCacheTimelife);
                        cacheItem = new SigningKeyCacheItem(signingKey, expiredAt);
                        _signingKeyCaches[cacheKey] = cacheItem;
                    }

                    signingKey = cacheItem.SigningKey;
                    break;

                case IdentitySigningKeyModes.Static:
                    signingKey = Setting.IssuerSigningKey.FirstOrDefault(x => x.Issuer == issuer)?.SigningKey;
                    if (signingKey.IsNullOrEmpty())
                        throw new Exception($"{nameof(Setting.IssuerSigningKey)} is not set or empty");
                    break;

                case IdentitySigningKeyModes.Jwks:
                    if (Setting.JwksEndpoint.IsNullOrEmpty())
                        throw new Exception($"{nameof(Setting.JwksEndpoint)} is not set");

                    var jwksCacheItem = GetValidCacheItem(cacheKey);
                    if (jwksCacheItem == null)
                    {
                        string jwksResponse;
                        if (isAsync)
                        {
                            await _jwksFetchLock.WaitAsync();
                            try
                            {
                                jwksCacheItem = GetValidCacheItem(cacheKey);
                                if (jwksCacheItem == null)
                                {
                                    jwksResponse = await _jwksHttpClient.GetStringAsync(Setting.JwksEndpoint);
                                    jwksCacheItem = CacheJwksSigningKey(tokenData, cacheKey, jwksResponse);
                                }
                            }
                            finally
                            {
                                _jwksFetchLock.Release();
                            }
                        }
                        else
                        {
                            jwksResponse = _jwksHttpClient.GetStringAsync(Setting.JwksEndpoint).GetAwaiter().GetResult();
                            jwksCacheItem = CacheJwksSigningKey(tokenData, cacheKey, jwksResponse);
                        }
                    }

                    signingKey = jwksCacheItem.SigningKey;
                    break;

                default:
                    throw new NotSupportedException($"SigningKeyMode {Setting.SigningKeyMode} không được hỗ trợ.");
            }

            if (tokenData.Header.Alg.IsNotNullOrEmpty() && tokenData.Header.Alg != Setting.Algorithm)
                throw new SecurityTokenInvalidAlgorithmException($"Token sử dụng algorithm {tokenData.Header.Alg} không khớp với algorithm {Setting.Algorithm} được cấu hình.");

            validationParameters.ValidAlgorithms = [Setting.Algorithm];
            switch (Setting.Algorithm)
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
                    throw new NotSupportedException($"Algorithm {Setting.Algorithm} không được hỗ trợ.");
            }
        }

        private SigningKeyCacheItem CacheJwksSigningKey(JwtSecurityToken tokenData, string cacheKey, string? jwksResponse)
        {
            if (jwksResponse.IsNullOrEmpty()) throw new Exception("jwks response is null or empty");

            var jwks = Newtonsoft.Json.Linq.JObject.Parse(jwksResponse);
            var keys = jwks["keys"] as Newtonsoft.Json.Linq.JArray;
            var tokenKid = tokenData.Header.Kid;

            var jwkItem = (!tokenKid.IsNullOrEmpty() ? keys?.FirstOrDefault(k => (string?)k["kid"] == tokenKid) : null)
                ?? keys?.FirstOrDefault(k => (string?)k["kid"] == tokenData.Issuer)
                ?? throw new Exception("jwk item is not found");

            var jwk = Newtonsoft.Json.JsonConvert.SerializeObject(jwkItem);
            var expiredAt = DateTime.UtcNow.AddSeconds(Setting.SigningKeyCacheTimelife);
            var jwksCacheItem = new SigningKeyCacheItem(jwk, expiredAt);
            _signingKeyCaches[cacheKey] = jwksCacheItem;

            return jwksCacheItem;
        }

        private class SigningKeyCacheItem(string signingKey, DateTime expiredAt)
        {
            public string SigningKey { get; set; } = signingKey;

            public DateTime ExpiredAt { get; set; } = expiredAt;
        }
    }
}
