using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TripleSix.Core.Helpers;
using TripleSix.Core.Identity;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Kiểm tra phải có scope chỉ định.
    /// </summary>
    public class RequireScope : TypeFilterAttribute
    {
        /// <summary>
        /// Kiểm tra phải có scope chỉ định.
        /// </summary>
        /// <param name="acceptedScope">Scope cho phép.</param>
        public RequireScope(string acceptedScope)
            : base(typeof(RequireScopeImplement))
        {
            Arguments = new object[] { acceptedScope };
        }
    }

    internal class RequireScopeImplement : IAuthorizationFilter
    {
        private readonly string _acceptedScope;

        public RequireScopeImplement(string acceptedScope)
        {
            if (acceptedScope.IsNullOrEmpty()) throw new ArgumentException("must not be null or white space only", nameof(acceptedScope));

            _acceptedScope = acceptedScope;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var scopeValue = context.HttpContext.User.FindFirstValue(nameof(IIdentityContext.Scope).ToCamelCase());
            if (scopeValue == null || !scopeValue.Split(' ').Any(x => x == _acceptedScope))
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}
