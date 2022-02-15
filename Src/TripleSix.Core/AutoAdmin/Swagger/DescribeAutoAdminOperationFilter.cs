using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using TripleSix.Core.Entities;
using TripleSix.Core.Helpers;
using TripleSix.Core.WebApi.Results;

namespace TripleSix.Core.AutoAdmin
{
    public class DescribeAutoAdminOperationFilter : IOperationFilter
    {
        private readonly Assembly _executingAssembly;

        public DescribeAutoAdminOperationFilter(Assembly executingAssembly)
        {
            _executingAssembly = executingAssembly;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ActionDescriptor is not ControllerActionDescriptor controllerDescriptor)
                return;

            var controllerType = controllerDescriptor.ControllerTypeInfo;
            if (!controllerType.IsGenericType) return;
            var entityType = controllerType.GetGenericArguments()[0];
            if (!typeof(IModelEntity).IsAssignableFrom(entityType)) return;

            var controllerBase = _executingAssembly.GetExportedTypes()
                .FirstOrDefault(t =>
                {
                    var metadata = t.GetCustomAttribute<AdminControllerAttribute>();
                    if (metadata is null) return false;
                    return metadata.AdminType is not null && metadata.EntityType == entityType;
                });
            if (controllerBase is null) return;

            var swaggerTag = controllerBase.GetCustomAttribute<SwaggerTagAttribute>();
            if (swaggerTag?.Description is not null && operation.Summary is not null)
                operation.Summary = Regex.Replace(operation.Summary, @"\[controller\]", swaggerTag.Description);

            if (controllerType.IsSubclassOfRawGeneric(typeof(BaseAdminControllerReadMethod<,,,>)))
            {
                if (controllerDescriptor.ActionName == "GetPage")
                {
                    var resultType = typeof(PagingResult<>)
                        .MakeGenericType(controllerType.GetGenericArguments()[2]);

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
            }

            context.SchemaRepository.Schemas.Clear();
        }
    }
}