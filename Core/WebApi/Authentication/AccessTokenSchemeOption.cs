using Microsoft.AspNetCore.Authentication;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Cấu hình xác thực bằng access token.
    /// </summary>
    public class AccessTokenSchemeOption : AuthenticationSchemeOptions
    {
        /// <summary>
        /// Key của header chứa access token.
        /// </summary>
        public string AccessTokenHeaderKey { get; set; } = "Authorization";
    }
}
