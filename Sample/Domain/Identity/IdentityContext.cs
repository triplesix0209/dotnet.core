using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TripleSix.Core.Helpers;
using TripleSix.Core.Identity;

namespace Sample.Domain.Identity
{
    public class IdentityContext : BaseIdentityContext
    {
        public IdentityContext(HttpContext? httpContext, IConfiguration configuration)
            : base(httpContext, configuration)
        {
        }

        protected override void ParseData(IEnumerable<Claim> claims)
        {
            Id = Guid.Parse(claims.FirstOrDefault(x => x.Type == nameof(Id).ToCamelCase())!.Value);
        }
    }
}
