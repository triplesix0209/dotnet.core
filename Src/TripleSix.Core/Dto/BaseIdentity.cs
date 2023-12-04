using System;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TripleSix.Core.Helpers;

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
            HttpContext = httpContext;

            if (httpContext.User.Identity.IsAuthenticated)
                User = httpContext.User;

            var properties = GetType().GetProperties().Where(x => x.CanWrite);
            foreach (var property in properties)
                property.SetValue(this, httpContext.Request.Headers.GetValue(property.Name));

            if (IpAddress.IsNullOrWhiteSpace())
                IpAddress = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (IpAddress.IsNullOrWhiteSpace())
                IpAddress = httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        public HttpContext HttpContext { get; }

        public ClaimsPrincipal User { get; }

        public virtual Guid? UserId
        {
            get
            {
                if (User == null || !User.Identity.IsAuthenticated)
                    return null;

                var identifier = User.Claims.FirstOrDefault(x => x.Type == "id");
                if (identifier == null) return null;

                return Guid.Parse(identifier.Value);
            }
        }

        [DisplayName("mã định danh client")]
        public string ClientId { get; set; }

        [DisplayName("địa chỉ ip")]
        public string IpAddress { get; set; }

        [DisplayName("url xử lý")]
        public string RequestUrl { get; set; }

        [DisplayName("ghi chú xử lý")]
        public string SubmitNote { get; set; }
    }
}
