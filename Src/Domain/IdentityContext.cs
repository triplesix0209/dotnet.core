using Microsoft.AspNetCore.Http;
using TripleSix.Core.Identity;

namespace Sample.Domain
{
    public class IdentityContext : BaseIdentityContext
    {
        public IdentityContext(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
        }
    }
}
