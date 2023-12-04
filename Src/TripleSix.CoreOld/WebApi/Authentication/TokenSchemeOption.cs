using Microsoft.AspNetCore.Authentication;

namespace TripleSix.CoreOld.WebApi.Authentication
{
    public class TokenSchemeOption : AuthenticationSchemeOptions
    {
        public string TokenHeaderKey { get; set; } = "Authorization";
    }
}
