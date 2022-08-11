using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.AutoAdmin
{
    public class DescribeAutoAdminOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ActionDescriptor is not ControllerActionDescriptor controllerDescriptor)
                return;

            var controllerType = controllerDescriptor.ControllerTypeInfo;
            if (!controllerType.IsGenericType) return;
            var adminModelType = controllerType.GetGenericArguments()[1];
            if (!adminModelType.IsSubclassOfRawGeneric(typeof(AdminModel<>))) return;

            var controllerBase = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsPublic && !x.IsAbstract)
                .Where(x => x.GetCustomAttribute<AdminControllerAttribute>()?.ModelType == adminModelType)
                .FirstOrDefault();
            if (controllerBase is null) return;

            var swaggerTag = controllerBase.GetCustomAttribute<SwaggerTagAttribute>();
            if (swaggerTag?.Description is not null && operation.Summary is not null)
                operation.Summary = Regex.Replace(operation.Summary, @"\[controller\]", swaggerTag.Description);
        }
    }
}
