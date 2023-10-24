using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TripleSix.Core.Identity;

namespace Sample.Domain.Identity
{
    public class IdentityContext : BaseIdentityContext
    {
        public IdentityContext(HttpContext? httpContext, IConfiguration configuration)
            : base(httpContext, configuration)
        {
        }
    }
}
