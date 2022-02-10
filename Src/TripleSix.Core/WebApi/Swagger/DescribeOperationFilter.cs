using System;
using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TripleSix.Core.Attributes;
using TripleSix.Core.Helpers;
using TripleSix.Core.WebApi.Results;

namespace TripleSix.Core.WebApi.Swagger
{
    public class DescribeOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var methodInfo = context.MethodInfo;
            if (methodInfo is null) return;
            var info = methodInfo
                .GetCustomAttributes(typeof(SwaggerApiAttribute), true)
                .FirstOrDefault() as SwaggerApiAttribute;

            operation.Summary = info?.Summary;
            operation.Description = info?.Description;

            var dataInstance = info is not null && info.ResponseType.GenericTypeArguments.Length == 1
                ? Activator.CreateInstance(info.ResponseType.GenericTypeArguments[0])
                : null;

            var responseType = new OpenApiMediaType();
            if (info is null)
            {
                responseType.Schema = typeof(SuccessResult).GenerateSchema(context.SchemaGenerator, context.SchemaRepository);
            }
            else if (dataInstance is null)
            {
                responseType.Schema = info.ResponseType.GenerateSchema(
                    context.SchemaGenerator,
                    context.SchemaRepository,
                    Activator.CreateInstance(info.ResponseType));
            }
            else
            {
                responseType.Schema = info.ResponseType.GenerateSchema(
                    context.SchemaGenerator,
                    context.SchemaRepository,
                    Activator.CreateInstance(info.ResponseType, new[] { dataInstance }));
            }

            var successResponse = new OpenApiResponse { Description = "Success" };
            successResponse.Content.Add("application/json", responseType);
            operation.Responses["200"] = successResponse;

            context.SchemaRepository.Schemas.Clear();
        }
    }
}