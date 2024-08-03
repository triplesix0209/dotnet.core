using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Kiểm tra phải có 1 trong các issuer chỉ định.
    /// </summary>
    public class RequireAnyIssuer : TypeFilterAttribute
    {
        /// <summary>
        /// Kiểm tra phải có 1 trong issuer chỉ định.
        /// </summary>
        /// <param name="acceptedIssuers">Danh sách issuer cho phép.</param>
        public RequireAnyIssuer(params string[] acceptedIssuers)
            : base(typeof(RequireAnyIssuerImplement))
        {
            Arguments = new object[] { acceptedIssuers };
        }
    }

    internal class RequireAnyIssuerImplement : IAuthorizationFilter
    {
        private readonly string[] _acceptedIssuers;

        public RequireAnyIssuerImplement(string[] acceptedIssuers)
        {
            if (acceptedIssuers.IsNullOrEmpty()) throw new ArgumentException("must have at least 1 issuer", nameof(acceptedIssuers));
            if (acceptedIssuers.Any(x => x.IsNullOrEmpty())) throw new ArgumentException("issuer must not be null or white space only", nameof(acceptedIssuers));

            _acceptedIssuers = acceptedIssuers;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var issuerValue = context.HttpContext.User.FindFirstValue("iss");
            if (issuerValue == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            var userIssuers = issuerValue.Split(' ');
            foreach (var acceptedIssuer in _acceptedIssuers)
            {
                if (userIssuers.Contains(acceptedIssuer))
                    return;
            }

            context.Result = new ForbidResult();
        }
    }
}
