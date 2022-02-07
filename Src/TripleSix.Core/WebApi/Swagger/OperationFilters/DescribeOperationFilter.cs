using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TripleSix.Core.Attributes;
using TripleSix.Core.WebApi.Results;

namespace TripleSix.Core.WebApi.Swagger
{
    public class DescribeOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var methodInfo = context.MethodInfo;
            if (methodInfo is null) return;
            var info = methodInfo.GetCustomAttributes(typeof(SwaggerApiAttribute), true).FirstOrDefault() as SwaggerApiAttribute;

            operation.Summary = info?.Summary;
            operation.Description = info?.Description;

            var successResponse = new OpenApiResponse { Description = "Success" };
            successResponse.Content.Add("application/json", new OpenApiMediaType
            {
                Schema = info is not null
                    ? context.SchemaGenerator.GenerateSchema(info.ResponseType, context.SchemaRepository)
                    : context.SchemaGenerator.GenerateSchema(typeof(SuccessResult), context.SchemaRepository),
            });
            operation.Responses["200"] = successResponse;

            var errorResponse = new OpenApiResponse { Description = "Error" };
            errorResponse.Content.Add("application/json", new OpenApiMediaType
            {
                Schema = context.SchemaGenerator.GenerateSchema(typeof(ErrorResult), context.SchemaRepository),
            });
            operation.Responses["500"] = errorResponse;
        }
    }
}