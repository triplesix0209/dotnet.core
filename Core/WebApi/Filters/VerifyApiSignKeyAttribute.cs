using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TripleSix.Core.Appsettings;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Attribute/Filter xác thực SignKey từ HTTP Request Header X-Sign (chạy ở giai đoạn Authorization).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class VerifyApiSignKeyAttribute : Attribute, IAsyncAuthorizationFilter, IOrderedFilter
    {
        private static readonly SemaphoreSlim _fetchLock = new(1, 1);
        private static readonly ConcurrentDictionary<string, PublicKeyCacheItem> _publicKeyCaches = new();
        private static readonly HttpClient _httpClient = new(new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(5),
        })
        {
            Timeout = TimeSpan.FromSeconds(10),
        };

        /// <summary>
        /// Tên header chứa SignKey, mặc định là X-Sign.
        /// </summary>
        public string HeaderName { get; set; } = "X-Sign";

        /// <summary>
        /// Thứ tự ưu tiên chạy filter (mặc định -1000 để chạy trước các Authorization filter).
        /// </summary>
        public int Order { get; set; } = -1000;

        /// <inheritdoc/>
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var request = context.HttpContext.Request;
            var signKeyHeaderValue = request.Headers[HeaderName].FirstOrDefault();

            if (signKeyHeaderValue.IsNullOrEmpty())
            {
                context.Result = new ObjectResult(new ErrorResult(401, "missing_sign_key", $"Thiếu header {HeaderName}"))
                {
                    StatusCode = 401,
                };
                return;
            }

            var setting = new ApiKeyAppsetting(context.HttpContext.RequestServices.GetRequiredService<IConfiguration>());
            JwtSecurityToken token;
            try
            {
                token = new JwtSecurityTokenHandler().ReadJwtToken(signKeyHeaderValue);
            }
            catch (Exception ex)
            {
                context.Result = new ObjectResult(new ErrorResult(401, "invalid_sign_key", $"Sign key không đúng định dạng JWT: {ex.Message}"))
                {
                    StatusCode = 401,
                };
                return;
            }

            var kid = token.Header.Kid;
            var appCode = kid.IsNotNullOrEmpty()
                ? (kid.StartsWith("api-key-") ? kid["api-key-".Length..] : kid)
                : (token.Issuer.IsNotNullOrEmpty() ? token.Issuer : setting.AppCode);
            if (appCode.IsNullOrEmpty())
            {
                context.Result = new ObjectResult(new ErrorResult(401, "invalid_sign_key", "Không xác định được AppCode."))
                {
                    StatusCode = 401,
                };
                return;
            }

            string? publicSignKey;
            try
            {
                publicSignKey = await GetPublicSignKeyAsync(setting, appCode);
            }
            catch (Exception ex)
            {
                context.Result = new ObjectResult(new ErrorResult(401, "public_key_fetch_error", $"Không thể lấy Public Key cho AppCode/ApiKeyId '{appCode}': {ex.Message}"))
                {
                    StatusCode = 401,
                };
                return;
            }

            if (publicSignKey.IsNullOrEmpty())
            {
                context.Result = new ObjectResult(new ErrorResult(401, "invalid_sign_key", $"Không tìm thấy Public Key cho AppCode/ApiKeyId '{appCode}'."))
                {
                    StatusCode = 401,
                };
                return;
            }

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(10),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidAlgorithms = [SecurityAlgorithms.EcdsaSha256],
                };

                if (publicSignKey.Contains("\"kty\""))
                {
                    validationParameters.IssuerSigningKey = new JsonWebKey(publicSignKey);
                }
                else
                {
                    var ecdsa = ECDsa.Create();
                    ecdsa.ImportFromPem(publicSignKey);
                    validationParameters.IssuerSigningKey = new ECDsaSecurityKey(ecdsa);
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(signKeyHeaderValue, validationParameters, out _);
                context.HttpContext.User = principal;
            }
            catch (SecurityTokenExpiredException)
            {
                context.Result = new ObjectResult(new ErrorResult(401, "sign_key_expired", "Sign key đã hết hạn."))
                {
                    StatusCode = 401,
                };
                return;
            }
            catch (Exception ex)
            {
                context.Result = new ObjectResult(new ErrorResult(401, "invalid_sign_key", $"Xác thực Sign key thất bại: {ex.Message}"))
                {
                    StatusCode = 401,
                };
                return;
            }
        }

        private static async Task<string?> GetPublicSignKeyAsync(ApiKeyAppsetting setting, string appCode)
        {
            var cacheKey = appCode;
            if (_publicKeyCaches.TryGetValue(cacheKey, out var cacheItem) && DateTime.UtcNow <= cacheItem.ExpiredAt)
                return cacheItem.PublicKey;

            await _fetchLock.WaitAsync();
            try
            {
                if (_publicKeyCaches.TryGetValue(cacheKey, out cacheItem) && DateTime.UtcNow <= cacheItem.ExpiredAt)
                    return cacheItem.PublicKey;

                var endpoint = setting.PublicKeyEndpoint.TrimEnd('/');
                var url = endpoint.Contains("{appCode}")
                    ? endpoint.Replace("{appCode}", appCode)
                    : $"{endpoint}/{appCode}";

                var publicKey = string.Empty;
                var responseString = await _httpClient.GetStringAsync(url);
                if (responseString.IsNotNullOrEmpty())
                {
                    try
                    {
                        using var doc = JsonDocument.Parse(responseString);
                        var root = doc.RootElement;
                        if (root.TryGetProperty("data", out var data) && data.ValueKind == JsonValueKind.String)
                            publicKey = data.GetString() ?? string.Empty;
                    }
                    catch
                    {
                        publicKey = responseString.Trim();
                    }
                }

                if (publicKey.IsNotNullOrEmpty())
                {
                    var cacheLifetime = setting.CacheLifetimeSeconds > 0 ? setting.CacheLifetimeSeconds : 900;
                    var expiredAt = DateTime.UtcNow.AddSeconds(cacheLifetime);
                    _publicKeyCaches[cacheKey] = new PublicKeyCacheItem(publicKey, expiredAt);
                }

                return publicKey;
            }
            finally
            {
                _fetchLock.Release();
            }
        }

        private class PublicKeyCacheItem(string publicKey, DateTime expiredAt)
        {
            public string PublicKey { get; } = publicKey;

            public DateTime ExpiredAt { get; } = expiredAt;
        }
    }
}
