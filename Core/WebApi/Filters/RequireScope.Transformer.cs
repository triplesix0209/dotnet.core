using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using TripleSix.Core.Helpers;
using TripleSix.Core.Identity;
using TripleSix.Core.Types;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Kiểm tra phải có scope chỉ định.
    /// </summary>
    /// <typeparam name="TScopeTransformer"><see cref="IScopeTransformer"/> đùng để tùy biến scope đầu vào.</typeparam>
    public class RequireScope<TScopeTransformer> : TypeFilterAttribute
        where TScopeTransformer : IScopeTransformer
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

        private class RequireScopeImplement : IAuthorizationFilter
        {
            private readonly string _acceptedScope;

            public RequireScopeImplement(string acceptedScope)
            {
                if (acceptedScope.IsNullOrEmpty()) throw new ArgumentException("must not be null or white space only", nameof(acceptedScope));

                _acceptedScope = acceptedScope;
            }

            public void OnAuthorization(AuthorizationFilterContext context)
            {
                var acceptedScope = _acceptedScope;
                if (Activator.CreateInstance(typeof(TScopeTransformer)) is IScopeTransformer transformer
                    && context.ActionDescriptor is ControllerActionDescriptor controllerDescriptor)
                    acceptedScope = transformer.Transform(acceptedScope, controllerDescriptor);

                var scopeValue = context.HttpContext.User.FindFirstValue(nameof(IIdentityContext.Scope).ToCamelCase());
                if (scopeValue == null || !scopeValue.Split(' ').Any(x => x == acceptedScope))
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }
        }
    }
}
