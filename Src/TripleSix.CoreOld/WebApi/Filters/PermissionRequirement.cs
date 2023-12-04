using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TripleSix.CoreOld.AutoAdmin;
using TripleSix.CoreOld.Enums;
using TripleSix.CoreOld.Helpers;

namespace TripleSix.CoreOld.WebApi.Filters
{
    public class PermissionRequirement : Attribute, IAuthorizationFilter
    {
        public bool AutoGroup { get; set; } = false;

        public PermissionOperators Operator { get; set; } = PermissionOperators.And;

        public string AccessLevelField { get; set; } = "AccessLevel";

        public string PermissionField { get; set; } = "Permission";

        public string Code { get; set; }

        public string[] ListCode { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            #region [kiểm tra xem đã xác thực chưa?]

            var user = context.HttpContext.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                context.Result = new ForbidResult();
                return;
            }

            #endregion

            #region [chuẩn bị permission code]

            var groupCode = string.Empty;
            if (AutoGroup)
            {
                var controllerName = context.ActionDescriptor.RouteValues["controller"] + "Controller";
                var controllerInfo = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .Where(x => x.IsPublic && !x.IsAbstract)
                    .Where(x => x.Name == controllerName)
                    .Where(x => x.GetCustomAttribute<AdminControllerAttribute>() is not null)
                    .FirstOrDefault()
                    .GetCustomAttribute<AdminControllerAttribute>();

                groupCode = controllerInfo.PermissionGroup.IsNullOrWhiteSpace()
                    ? context.ActionDescriptor.RouteValues["controller"].ToCamelCase() + "."
                    : controllerInfo.PermissionGroup + ".";
            }

            var permissionCodes = new List<string>();
            if (Code.IsNotNullOrWhiteSpace())
                permissionCodes.Add(groupCode + Code);
            if (ListCode.IsNotNullOrEmpty())
            {
                permissionCodes.AddRange(ListCode
                    .Where(x => x.IsNotNullOrWhiteSpace())
                    .Select(x => groupCode + x));
            }

            if (permissionCodes.IsNullOrEmpty())
                return;

            #endregion

            #region [kiểm tra cấp độ tài khoản]

            var accessLevel = Convert.ToInt32(user.Claims.FirstOrDefault(x => x.Type == AccessLevelField.ToCamelCase())?.Value);
            if (accessLevel == 0) return;

            #endregion

            #region [kiểm tra permission]

            var userListPermission = user.Claims.FirstOrDefault(x => x.Type == PermissionField.ToCamelCase())?.Value;
            if (userListPermission.IsNullOrWhiteSpace())
            {
                context.Result = new ForbidResult();
                return;
            }

            var userPermissions = userListPermission.Split(" ");
            if (Operator == PermissionOperators.And && permissionCodes.Any(code => !userPermissions.Contains(code)))
            {
                context.Result = new ForbidResult();
                return;
            }
            else if (Operator == PermissionOperators.Or && permissionCodes.All(code => !userPermissions.Contains(code)))
            {
                context.Result = new ForbidResult();
                return;
            }

            #endregion
        }
    }
}
