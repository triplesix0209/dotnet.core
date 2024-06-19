using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
            if (Setting.BypassUserIds.IsNotNullOrEmpty())
            {
                var tokenData = ReadJwtToken(token);
                var userId = tokenData.Claims.FindFirstValue(nameof(IIdentityContext.Id).ToCamelCase());
                if (userId.IsNotNullOrEmpty() && Setting.BypassUserIds.Contains(userId))
                {
                    validationParameters.ValidateLifetime = false;
                }
            }

            switch (Setting.SigningKeyMode)
            {
                case IdentitySigningKeyModes.Static:
                    if (Setting.SigningKey.IsNullOrEmpty())
                        throw new ArgumentNullException(nameof(Setting.SigningKey));

                    validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Setting.SigningKey));
                    break;

                case IdentitySigningKeyModes.Dynamic:
                    if (GetSigningKeyMethod == null)
                        throw new ArgumentNullException(nameof(GetSigningKeyMethod));

                    var tokenData = ReadJwtToken(token);
                    var issuer = tokenData.Issuer;

                    var signingKeyCacheItem = _signingKeyCaches.ContainsKey(issuer) ? _signingKeyCaches[issuer] : null;
                    if (signingKeyCacheItem == null || DateTime.UtcNow > signingKeyCacheItem.ExpiredAt)
                    {
                        var signingKey = GetSigningKeyMethod(Setting, tokenData);
                        if (signingKey.IsNullOrEmpty()) throw new ArgumentNullException(nameof(signingKey));
                        var expiredAt = DateTime.UtcNow.AddSeconds(Setting.SigningKeyCacheTimelife ?? 0);
                        signingKeyCacheItem = new SigningKeyCacheItem(signingKey, expiredAt);
                        _signingKeyCaches[issuer] = signingKeyCacheItem;
                    }

                    validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKeyCacheItem.SigningKey));
                    break;
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
