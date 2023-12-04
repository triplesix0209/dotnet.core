using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.WebApi.Filters
{
    public class AccessLevelRequirement : Attribute, IAuthorizationFilter
    {
        public int MinimumAccountLevel { get; set; } = 0;

        public string AccessLevelField { get; set; } = "AccessLevel";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                context.Result = new ForbidResult();
                return;
            }

            var accessLevel = Convert.ToInt32(user.Claims.FirstOrDefault(x => x.Type == AccessLevelField.ToCamelCase())?.Value);
            if (accessLevel > MinimumAccountLevel)
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}
