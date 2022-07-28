using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TripleSix.Core.Identity;

namespace TripleSix.Core.WebApi
{
    public class AccessLevelRequirement : Attribute, IAuthorizationFilter
    {
        public int MinAccountLevel { get; set; } = 0;

        public int MaxAccountLevel { get; set; } = 99;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var identity = new BaseIdentityContext(context.HttpContext);

            if (identity.AccessLevel < MinAccountLevel || identity.AccessLevel > MaxAccountLevel)
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}
