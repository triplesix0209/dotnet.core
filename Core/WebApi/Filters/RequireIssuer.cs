using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Kiểm tra phải có issuer chỉ định.
    /// </summary>
    public class RequireIssuer : TypeFilterAttribute
    {
        /// <summary>
        /// Kiểm tra phải có issuer chỉ định.
        /// </summary>
        /// <param name="acceptedIssuer">Issuer cho phép.</param>
        public RequireIssuer(string acceptedIssuer)
            : base(typeof(RequireIssuerImplement))
        {
            Arguments = new object[] { acceptedIssuer };
        }
    }

    internal class RequireIssuerImplement : IAuthorizationFilter
    {
        private readonly string _acceptedIssuer;

        public RequireIssuerImplement(string acceptedIssuer)
        {
            if (acceptedIssuer.IsNullOrEmpty()) throw new ArgumentException("must not be null or white space only", nameof(acceptedIssuer));

            _acceptedIssuer = acceptedIssuer;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var issuerValue = context.HttpContext.User.FindFirstValue("iss");
            if (issuerValue == null || !issuerValue.Split(' ').Any(x => x == _acceptedIssuer))
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}
