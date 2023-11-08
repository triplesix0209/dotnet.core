#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly

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
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ActionDescriptor is not ControllerActionDescriptor controllerInfo) return;
            var methodInfo = controllerInfo.MethodInfo;
            if (methodInfo == null) return;

            #region [parameter]

            operation.Parameters.Clear();
            operation.RequestBody = new OpenApiRequestBody();

            foreach (var parameterDescription in context.ApiDescription.ParameterDescriptions)
            {
                if (parameterDescription.Type == null) continue;

                var parameterLocation = parameterDescription.Source.DisplayName;
                if (parameterLocation == "ModelBinding") parameterLocation = "Query";

                if (parameterLocation == "Body")
                {
                    if (!operation.RequestBody.Content.ContainsKey("application/json"))
                        operation.RequestBody.Content.Add("application/json", new OpenApiMediaType());
                    var bodyContent = operation.RequestBody.Content["application/json"];

                    var instance = Activator.CreateInstance(parameterDescription.Type);
                    bodyContent.Schema = parameterDescription.Type.GenerateSwaggerSchema(
                        context.SchemaGenerator,
                        context.SchemaRepository,
                        defaultValue: instance,
                        generateDefaultValue: true);
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

                    PropertyInfo? parentPropertyInfo = null;
                    if (parameterDescription.Name.Contains('.'))
                        parentPropertyInfo = parameterDescription.ParameterDescriptor.ParameterType.GetProperty(parameterDescription.Name.Split(".")[0]);

                    var modelType = parameterDescription.ModelMetadata.ContainerType;
                    if (modelType == null) continue;
                    var instance = Activator.CreateInstance(modelType);
                    parameter.Schema = parameterDescription.Type.GenerateSwaggerSchema(
                        context.SchemaGenerator,
                        context.SchemaRepository,
                        propertyInfo: propertyInfo,
                        parentPropertyInfo: parentPropertyInfo,
                        defaultValue: propertyInfo.GetValue(instance),
                        generateDefaultValue: true);

                    parameter.Name = parameterDescription.Name.Split(".").Select(x => x.ToCamelCase()).ToString(".");
                    parameter.Required = parameter.In == ParameterLocation.Path ||
                            (propertyInfo?.GetCustomAttribute<RequiredAttribute>() != null && parameter.Schema.Default is OpenApiNull);

                    operation.Parameters.Add(parameter);
                }
            }

            #endregion

            #region [response]

            var returnType = methodInfo.ReturnType;
            returnType = returnType != null && returnType.IsSubclassOfOpenGeneric(typeof(Task<>))
                ? returnType.GetGenericArguments()[0]
                : typeof(SuccessResult);

            var responseType = new OpenApiMediaType();
            responseType.Schema = returnType.GenerateSwaggerSchema(
                context.SchemaGenerator,
                context.SchemaRepository,
                generateDefaultValue: false);

            var successResponse = new OpenApiResponse { Description = "Success" };
            successResponse.Content.Add("application/json", responseType);
            operation.Responses["200"] = successResponse;

            #endregion

            #region [authorization]

            var authorizeAttr = methodInfo.GetCustomAttribute<AuthorizeAttribute>(true)
                ?? controllerInfo.ControllerTypeInfo.GetCustomAttribute<AuthorizeAttribute>(true);

            if (authorizeAttr != null)
            {
                var requireScopeAttrs = methodInfo.GetCustomAttributes<RequireScope>(true).ToList();
                requireScopeAttrs.AddRange(controllerInfo.ControllerTypeInfo.GetCustomAttributes<RequireScope>(true));
                var requireAnyScopeAttrs = methodInfo.GetCustomAttributes<RequireAnyScope>(true).ToList();
                requireAnyScopeAttrs.AddRange(controllerInfo.ControllerTypeInfo.GetCustomAttributes<RequireAnyScope>(true));
                var requireScopes = new HashSet<string>();

                foreach (var requireScopeAttr in requireScopeAttrs)
                {
                    var scope = requireScopeAttr.Arguments?[0].ToString();
                    if (scope.IsNullOrEmpty()) continue;
                    requireScopes.Add(scope);
                }

                foreach (var requireScopeAttr in requireAnyScopeAttrs)
                {
                    var scopes = requireScopeAttr.Arguments?.SelectMany(x => (string[])x);
                    if (scopes.IsNullOrEmpty()) continue;

                    if (scopes.Count() == 1) requireScopes.Add(scopes.First());
                    else requireScopes.Add($"[{scopes.ToString(", ")}]");
                }

                operation.Security.Add(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "Bearer",
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "AccessToken",
                            },
                        },
                        requireScopes.ToList()
                    }
                });
            }

            #endregion

            operation.OperationId = controllerInfo.ActionName + controllerInfo.ControllerName;
            if (operation.RequestBody.Content.Count == 0)
                operation.RequestBody = null;
            context.SchemaRepository.Schemas.Clear();
        }
    }
}
