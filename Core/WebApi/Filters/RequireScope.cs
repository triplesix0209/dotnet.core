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
        /// <param name="acceptedScopes">Danh sách Scope cho phép, chỉ cần có một.</param>
        public RequireScope(params string[] acceptedScopes)
            : base(typeof(RequireScopeImplement))
        {
            Arguments = new object[] { acceptedScopes };
        }

        private class RequireScopeImplement : IAuthorizationFilter
        {
            private readonly string[] _acceptedScopes;

            public RequireScopeImplement(string[] acceptedScopes)
            {
                _acceptedScopes = acceptedScopes ?? throw new ArgumentNullException(nameof(_acceptedScopes));
                if (_acceptedScopes.Length == 0) throw new ArgumentException("must have at least 1 item", nameof(acceptedScopes));
            }

            public void OnAuthorization(AuthorizationFilterContext context)
            {
                var userScopeValue = context.HttpContext.User.FindFirstValue(nameof(IIdentityContext.Scope).ToCamelCase());
                if (userScopeValue == null)
                {
                    context.Result = new ForbidResult();
                    return;
                }

                var userScopes = userScopeValue.Split(' ');
                foreach (var acceptedScope in _acceptedScopes)
                {
                    if (userScopes.Any(s => s == acceptedScope))
                        return;
                }

                context.Result = new ForbidResult();
            }
        }
    }
}
