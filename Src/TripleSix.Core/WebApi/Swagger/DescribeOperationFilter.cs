using System.Linq;
using System.Reflection;
using Microsoft.OpenApi.Any;
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

            var apiInfo = methodInfo.GetCustomAttributes(typeof(SwaggerApiAttribute), true)
                .FirstOrDefault() as SwaggerApiAttribute;

            #region [parameter]

            operation.Parameters.Clear();
            operation.RequestBody = new OpenApiRequestBody();

            foreach (var parameterDescription in context.ApiDescription.ParameterDescriptions)
            {
                var parameterLocation = parameterDescription.Source.DisplayName;
                if (parameterLocation == "ModelBinding")
                {
                    switch (context.ApiDescription.HttpMethod)
                    {
                        case "GET":
                        case "DELETE":
                            parameterLocation = "Query";
                            break;

                        case "POST":
                        case "PUT":
                            parameterLocation = "Body";
                            break;
                    }
                }

                if (parameterLocation == "Body")
                {
                    OpenApiMediaType bodyContent;
                    if (operation.RequestBody.Content.ContainsKey("application/json"))
                    {
                        bodyContent = operation.RequestBody.Content["application/json"];
                    }
                    else
                    {
                        operation.RequestBody.Content.Add("application/json", new OpenApiMediaType());
                        bodyContent = operation.RequestBody.Content["application/json"];
                    }

                    bodyContent.Schema = parameterDescription.Type.GenerateSchema(
                        context.SchemaGenerator,
                        context.SchemaRepository,
                        generateDefault: true,
                        defaultInstance: parameterDescription.Type.CreateDefaultInstance());
                }
                else
                {
                    var parameter = new OpenApiParameter();
                    switch (parameterLocation)
                    {
                        case "Path":
                            parameter.In = ParameterLocation.Path;
                            break;
                        case "Query":
                            parameter.In = ParameterLocation.Query;
                            break;
                        case "Header":
                            parameter.In = ParameterLocation.Header;
                            break;
                    }

                    var propertyInfo = parameterDescription.PropertyInfo();
                    if (propertyInfo is null) continue;

                    PropertyInfo parentPropertyInfo = null;
                    if (parameterDescription.Name.Contains("."))
                    {
                        parentPropertyInfo = parameterDescription.ParameterDescriptor.ParameterType
                            .GetProperty(parameterDescription.Name.Split(".")[0]);
                    }

                    parameter.Name = string.Join(".", parameterDescription.Name.Split(".")
                        .Select(x => x.ToCamelCase()));
                    parameter.Schema = propertyInfo.PropertyType.GenerateSchema(
                        context.SchemaGenerator,
                        context.SchemaRepository,
                        generateDefault: true,
                        defaultInstance: propertyInfo.GetValue(propertyInfo.DeclaringType.CreateDefaultInstance()),
                        propertyInfo: propertyInfo,
                        parentPropertyInfo: parentPropertyInfo);

                    if (parameter.In == ParameterLocation.Path)
                    {
                        parameter.Required = true;
                    }
                    else
                    {
                        parameter.Required = propertyInfo.GetCustomAttribute<RequiredValidateAttribute>() is not null
                            && parameter.Schema.Default is OpenApiNull;
                    }

                    operation.Parameters.Add(parameter);
                }
            }

            #endregion

            #region [response]

            var responseType = new OpenApiMediaType();
            responseType.Schema = (apiInfo is null ? typeof(SuccessResult) : apiInfo.ResponseType).GenerateSchema(
                context.SchemaGenerator,
                context.SchemaRepository,
                generateDefault: false);

            var successResponse = new OpenApiResponse { Description = "Success" };
            successResponse.Content.Add("application/json", responseType);
            operation.Responses["200"] = successResponse;
            operation.Summary = apiInfo?.Summary;
            operation.Description = apiInfo?.Description;

            #endregion

            context.SchemaRepository.Schemas.Clear();
        }
    }
}