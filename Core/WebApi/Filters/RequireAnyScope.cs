using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TripleSix.Core.Helpers;
using TripleSix.Core.Identity;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Kiểm tra phải có 1 trong các scope chỉ định.
    /// </summary>
    public class RequireAnyScope : TypeFilterAttribute
    {
        /// <summary>
        /// Kiểm tra phải có 1 trong scope chỉ định.
        /// </summary>
        /// <param name="acceptedScopes">Danh sách Scope cho phép.</param>
        public RequireAnyScope(params string[] acceptedScopes)
            : base(typeof(RequireAnyScopeImplement))
        {
            Arguments = new object[] { acceptedScopes };
        }
    }

    internal class RequireAnyScopeImplement : IAuthorizationFilter
    {
        private readonly string[] _acceptedScopes;

        public RequireAnyScopeImplement(string[] acceptedScopes)
        {
            if (acceptedScopes.IsNullOrEmpty()) throw new ArgumentException("must have at least 1 scope", nameof(acceptedScopes));
            if (acceptedScopes.Any(x => x.IsNullOrEmpty())) throw new ArgumentException("scope must not be null or white space only", nameof(acceptedScopes));

            _acceptedScopes = acceptedScopes;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var scopeValue = context.HttpContext.User.FindFirstValue(nameof(IIdentityContext.Scope).ToCamelCase());
            if (scopeValue == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            var userScopes = scopeValue.Split(' ');
            foreach (var acceptedScope in _acceptedScopes)
            {
                if (userScopes.Contains(acceptedScope))
                    return;
            }

            context.Result = new ForbidResult();
        }
    }
}
