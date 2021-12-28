using System;
using System.Linq;
using System.Security.Claims;
using CpTech.Core.Enums;
using CpTech.Core.Extensions;
using CpTech.Core.Helpers;
using Microsoft.AspNetCore.Http;

namespace CpTech.Core.Dto
{
    public class BaseIdentity
        : IIdentity
    {
        public BaseIdentity()
        {
            User = null;
            ClientDeviceType = ClientDeviceType.Unknown;
        }

        public BaseIdentity(ClaimsPrincipal user)
        {
            User = user;
        }

        public BaseIdentity(HttpContext httpContext)
        {
            var header = httpContext.Request.Headers;

            ClientDeviceType = httpContext.Request.Headers.GetHeaderValue(
                nameof(IIdentity.ClientDeviceType).ToKebabCase(),
                value => EnumHelper.Parse<ClientDeviceType>(value),
                ClientDeviceType.Unknown);

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

        public ClientDeviceType ClientDeviceType { get; }
    }
}