using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TripleSix.Core.Identity;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Kiểm tra cấp độ tài khoản.
    /// </summary>
    public class AccessLevelRequirement : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// Cấp độ tài khoản tối thiểu cho phép.
        /// </summary>
        public int MinimumAccountLevel { get; set; } = 0;

        /// <summary>
        /// Field để lấy cập độ tài khoản từ <see cref="IIdentityContext"/>.
        /// </summary>
        public string AccessLevelField { get; set; } = nameof(BaseIdentityContext.AccessLevel);

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var identity = new BaseIdentityContext(context.HttpContext);

            if (!identity.IsAuthorized)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (identity.AccessLevel > MinimumAccountLevel)
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}
