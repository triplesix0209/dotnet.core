#pragma warning disable SA1649 // File name should match first type name

using Microsoft.AspNetCore.Authentication;

namespace TripleSix.CoreOld.WebApi.Authentication
{
    public static class AuthenticationExtension
    {
        public static AuthenticationBuilder AddTokenScheme(this AuthenticationBuilder builder, string authenticationScheme)
        {
            return builder.AddScheme<TokenSchemeOption, TokenSchemeHandler>(authenticationScheme, option => { });
        }
    }
}
