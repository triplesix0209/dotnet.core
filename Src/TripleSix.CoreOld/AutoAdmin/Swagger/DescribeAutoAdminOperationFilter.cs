using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using TripleSix.CoreOld.Helpers;
using TripleSix.CoreOld.WebApi.Results;

namespace TripleSix.CoreOld.AutoAdmin
{
    public class DescribeAutoAdminOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ActionDescriptor is not ControllerActionDescriptor controllerDescriptor)
                return;

            var controllerType = controllerDescriptor.ControllerTypeInfo;
            if (!controllerType.IsGenericType) return;
            var adminType = controllerType.GetGenericArguments()[0];
            if (!adminType.IsAssignableTo<IAdminDto>()) return;

            var controllerBase = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsPublic && !x.IsAbstract)
                .Where(x => x.GetCustomAttribute<AdminControllerAttribute>()?.AdminType == adminType)
                .FirstOrDefault();
            if (controllerBase is null) return;

            var swaggerTag = controllerBase.GetCustomAttribute<SwaggerTagAttribute>();
            if (swaggerTag?.Description is not null && operation.Summary is not null)
                operation.Summary = Regex.Replace(operation.Summary, @"\[controller\]", swaggerTag.Description);

            if (controllerType.IsSubclassOfRawGeneric(typeof(BaseAdminControllerReadMethod<,,,,>)))
            {
                if (controllerDescriptor.ActionName == "GetPage")
                {
                    var resultType = typeof(PagingResult<>)
                        .MakeGenericType(controllerType.GetGenericArguments()[3]);

                    var responseType = new OpenApiMediaType();
                    responseType.Schema = resultType.GenerateSchema(
                        context.SchemaGenerator,
                        context.SchemaRepository,
                        generateDefault: false);

                    var successResponse = new OpenApiResponse { Description = "Success" };
                    successResponse.Content.Add("application/json", responseType);
                    operation.Responses["200"] = successResponse;
                }
                else if (controllerDescriptor.ActionName == "GetDetail")
                {
                    var resultType = typeof(DataResult<>)
                        .MakeGenericType(controllerType.GetGenericArguments()[4]);

                    var responseType = new OpenApiMediaType();
                    responseType.Schema = resultType.GenerateSchema(
                        context.SchemaGenerator,
                        context.SchemaRepository,
                        generateDefault: false);

                    var successResponse = new OpenApiResponse { Description = "Success" };
                    successResponse.Content.Add("application/json", responseType);
                    operation.Responses["200"] = successResponse;
                }
            }

            context.SchemaRepository.Schemas.Clear();
        }
    }
}
