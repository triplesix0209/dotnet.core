using Microsoft.AspNetCore.Authentication;

namespace TripleSix.Core.WebApi.Authentication
{
    public class TokenSchemeOption : AuthenticationSchemeOptions
    {
        public string TokenHeaderKey { get; set; } = "Authorization";
    }
}
