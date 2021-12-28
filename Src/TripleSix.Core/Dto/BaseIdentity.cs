using System;
using System.Linq;
using System.Security.Claims;
using TripleSix.Core.Extensions;
using Microsoft.AspNetCore.Http;

namespace TripleSix.Core.Dto
{
    public class BaseIdentity
        : IIdentity
    {
        public BaseIdentity()
        {
            User = null;
        }

        public BaseIdentity(ClaimsPrincipal user)
        {
            User = user;
        }

        public BaseIdentity(HttpContext httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                User = httpContext.User;
            }
        }

        public ClaimsPrincipal User { get; }

        public virtual Guid? UserId
        {
            get
            {
                if (User == null || !User.Identity.IsAuthenticated)
                    return null;

                var id = User.Claims.FirstOrDefault(x => x.Type == "id");
                if (id == null) return null;

                return Guid.Parse(id.Value);
            }
        }
    }
}