using Microsoft.AspNetCore.Http;
using TripleSix.Core.Identity;

namespace Sample.Domain.Identity
{
    public class IdentityContext : BaseIdentityContext
    {
        public IdentityContext(HttpContext? httpContext)
            : base(httpContext)
        {
            var tokenData = GetAccessTokenData(httpContext);
            if (tokenData == null) return;

            var nameClaim = tokenData.Claims.GetClaim(nameof(IdentityProfileDto.Name));
            if (nameClaim != null)
                Name = nameClaim.Value;

            var usernameClaim = tokenData.Claims.GetClaim(nameof(IdentityProfileDto.Username));
            if (usernameClaim != null)
                Username = usernameClaim.Value;
        }

        public string Name { get; set; }

        public string Username { get; set; }
    }
}
