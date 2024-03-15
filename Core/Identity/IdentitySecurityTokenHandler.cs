using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TripleSix.Core.Appsettings;
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
        public Func<IdentityAppsetting, JwtSecurityToken, string>? GetSigningKeyMethod { get; set; }

        /// <inheritdoc/>
        public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            if (GetSigningKeyMethod != null)
            {
                var data = ReadJwtToken(token);
                var signingKey = GetSigningKeyMethod(Setting, data);
                if (signingKey.IsNullOrEmpty()) throw new Exception("signing key is not found");
                validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
            }
            else
            {
                validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Setting.SigningKey!));
            }

            return base.ValidateToken(token, validationParameters, out validatedToken);
        }
    }
}
