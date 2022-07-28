using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.WebApi
{
    public class DescribeOperationFilter : IOperationFilter
    {
        private string _defaultSecurityScheme;

        public DescribeOperationFilter(string defaultSecurityScheme)
        {
            _defaultSecurityScheme = defaultSecurityScheme;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ActionDescriptor is not ControllerActionDescriptor controllerInfo) return;
            var methodInfo = controllerInfo.MethodInfo;
            if (methodInfo == null) return;
            var apiInfo = methodInfo.GetCustomAttribute<SwaggerApiAttribute>(true);
            if (apiInfo == null) apiInfo = new SwaggerApiAttribute(typeof(SuccessResult));

            #region [parameter]

            operation.Parameters.Clear();
            operation.RequestBody = new OpenApiRequestBody();

            foreach (var parameterDescription in context.ApiDescription.ParameterDescriptions)
            {
                if (parameterDescription.Type == null) continue;

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
                    if (!operation.RequestBody.Content.ContainsKey("application/json"))
                        operation.RequestBody.Content.Add("application/json", new OpenApiMediaType());
                    var bodyContent = operation.RequestBody.Content["application/json"];

                    bodyContent.Schema = parameterDescription.Type.GenerateSwaggerSchema(
                        context.SchemaGenerator,
                        context.SchemaRepository,
                        parameterDescription);
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
                    parameter.Schema = parameterDescription.Type.GenerateSwaggerSchema(
                        context.SchemaGenerator,
                        context.SchemaRepository,
                        parameterDescription,
                        propertyInfo: propertyInfo);

                    parameter.Name = parameterDescription.Name.Split(".").Select(x => x.ToCamelCase()).ToString(".");
                    parameter.Required = parameter.In == ParameterLocation.Path ||
                            (propertyInfo?.GetCustomAttribute<RequiredAttribute>() != null && parameter.Schema.Default is OpenApiNull);

                    operation.Parameters.Add(parameter);
                }
            }

            #endregion

            #region [response]

            var responseType = new OpenApiMediaType();
            responseType.Schema = apiInfo.ResponseType.GenerateSwaggerSchema(
                context.SchemaGenerator,
                context.SchemaRepository,
                generateDefault: false);

            var successResponse = new OpenApiResponse { Description = "Success" };
            successResponse.Content.Add("application/json", responseType);
            operation.Responses["200"] = successResponse;
            operation.Summary = apiInfo?.Summary;
            operation.Description = apiInfo?.Description;

            #endregion

            #region [authentication]

            if (controllerInfo.EndpointMetadata.FirstOrDefault(x => x.GetType() == typeof(AuthorizeAttribute)) is AuthorizeAttribute authorize)
            {
                var securityScheme = authorize.AuthenticationSchemes.IsNullOrWhiteSpace()
                    ? _defaultSecurityScheme
                    : authorize.AuthenticationSchemes;

                var options = new List<string>();
                if (controllerInfo.EndpointMetadata.FirstOrDefault(x => x.GetType() == typeof(AllowAnonymousAttribute)) is AllowAnonymousAttribute allowAnonymous)
                    options.Add("không bắt buộc");

                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = securityScheme,
                            },
                        },
                        options
                    },
                });
            }

            #endregion

            context.SchemaRepository.Schemas.Clear();
        }
    }
}
