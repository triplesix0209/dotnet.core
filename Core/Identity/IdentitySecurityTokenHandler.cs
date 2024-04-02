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
                validationParameters.ValidateLifetime = userId == null || !Setting.BypassUserIds.Contains(userId);
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
                    var signingKey = GetSigningKeyMethod(Setting, tokenData);
                    if (signingKey.IsNullOrEmpty()) throw new ArgumentNullException(nameof(signingKey));
                    validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
                    break;
            }

            return base.ValidateToken(token, validationParameters, out validatedToken);
        }
    }
}
