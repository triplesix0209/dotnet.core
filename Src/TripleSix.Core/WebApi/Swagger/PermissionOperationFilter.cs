using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TripleSix.Core.AutoAdmin;
using TripleSix.Core.Enums;
using TripleSix.Core.Helpers;
using TripleSix.Core.WebApi.Filters;

namespace TripleSix.Core.WebApi.Swagger
{
    public class PermissionOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ActionDescriptor is not ControllerActionDescriptor controllerDescriptor)
                return;

            var action = controllerDescriptor;
            var permission = action.MethodInfo.GetCustomAttribute<PermissionRequirement>();
            if (permission is null) return;

            var groupCode = string.Empty;
            if (permission.AutoGroup)
            {
                var controllerName = action.ControllerName + "Controller";
                var controllerInfo = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .Where(x => x.IsPublic && !x.IsAbstract)
                    .Where(x => x.Name == controllerName)
                    .Where(x => x.GetCustomAttribute<AdminControllerAttribute>() is not null)
                    .FirstOrDefault()
                    .GetCustomAttribute<AdminControllerAttribute>();

                groupCode = controllerInfo.PermissionGroup.IsNullOrWhiteSpace()
                    ? action.ControllerName.ToCamelCase() + "."
                    : controllerInfo.PermissionGroup + ".";

                var listCodes = new List<string>();
                if (permission.Code.IsNotNullOrWhiteSpace())
                    listCodes.Add(groupCode + permission.Code);
                if (permission.ListCode.IsNotNullOrEmpty())
                {
                    listCodes.AddRange(permission.ListCode
                        .Where(x => x.IsNotNullOrWhiteSpace())
                        .Select(x => groupCode + x));
                }

                if (listCodes.IsNullOrEmpty()) return;

                var permissionText = listCodes.Count == 1
                    ? $"yêu cầu quyền: <b>{listCodes[0]}</b>"
                    : $"yêu cầu {(permission.Operator == PermissionOperators.And ? "tất cả các" : "một trong")} quyền: <b>{string.Join(", ", listCodes)}</b>";

                operation.Description = permissionText
                    + (operation.Description.IsNotNullOrWhiteSpace() ? "<br/>" : string.Empty)
                    + operation.Description;
            }
        }
    }
}
