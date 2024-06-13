#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly

using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TripleSix.Core.Helpers;
using TripleSix.Core.Types;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Describe operation filter.
    /// </summary>
    public class DescribeOperationFilter : IOperationFilter
    {
        /// <inheritdoc/>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ActionDescriptor is not ControllerActionDescriptor controllerDescriptor) return;
            var baseController = controllerDescriptor.ControllerTypeInfo.BaseType;
            if (baseController is null) return;
            var methodInfo = controllerDescriptor.MethodInfo;
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
                    bodyContent.Schema.Nullable = false;
                }
                else if (parameterLocation == "FormFile")
                {
                    if (!operation.RequestBody.Content.ContainsKey("multipart/form-data"))
                        operation.RequestBody.Content.Add("multipart/form-data", new OpenApiMediaType());
                    var bodyContent = operation.RequestBody.Content["multipart/form-data"];

                    bodyContent.Schema = parameterDescription.ParameterDescriptor.ParameterType.GenerateSwaggerSchema(
                        context.SchemaGenerator,
                        context.SchemaRepository,
                        generateDefaultValue: false);
                    bodyContent.Schema.Nullable = false;
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

                    var modelType = parameterDescription.ModelMetadata.ContainerType;
                    if (modelType == null) continue;

                    var instance = Activator.CreateInstance(modelType);
                    var propertyInfo = parameterDescription.PropertyInfo();
                    PropertyInfo? parentPropertyInfo = null;
                    if (parameterDescription.Name.Contains('.'))
                        parentPropertyInfo = parameterDescription.ParameterDescriptor.ParameterType.GetProperty(parameterDescription.Name.Split(".")[0]);

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

            if (operation.RequestBody.Content.Count == 0)
                operation.RequestBody = null;
            else
                operation.RequestBody.Required = true;

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
                ?? controllerDescriptor.ControllerTypeInfo.GetCustomAttribute<AuthorizeAttribute>(true);

            if (authorizeAttr != null)
            {
                var requireScopes = new HashSet<string>();
                var requireIssuers = new HashSet<string>();

                var requireScopeAttrs = methodInfo.GetCustomAttributes<RequireScope>(true).ToList();
                requireScopeAttrs.AddRange(controllerDescriptor.ControllerTypeInfo.GetCustomAttributes<RequireScope>(true));
                foreach (var requireScopeAttr in requireScopeAttrs)
                {
                    var scope = requireScopeAttr.Arguments?[0].ToString();
                    if (scope.IsNullOrEmpty()) continue;
                    requireScopes.Add(scope);
                }

                var requireScopeTransformerAttrs = methodInfo.GetCustomAttributes(typeof(RequireScope<>), true).ToList();
                foreach (var requireScopeTransformerAttr in requireScopeTransformerAttrs)
                {
                    var typeFilter = requireScopeTransformerAttr as TypeFilterAttribute;
                    var inputScope = typeFilter?.Arguments?[0] as string;
                    if (inputScope.IsNullOrEmpty()) continue;
                    if (Activator.CreateInstance(requireScopeTransformerAttr.GetType().GetGenericArguments()[0]) is not IScopeTransformer transformer)
                        continue;

                    requireScopes.Add(transformer.Transform(inputScope, controllerDescriptor));
                }

                var requireAnyScopeAttrs = methodInfo.GetCustomAttributes<RequireAnyScope>(true).ToList();
                requireAnyScopeAttrs.AddRange(controllerDescriptor.ControllerTypeInfo.GetCustomAttributes<RequireAnyScope>(true));
                foreach (var requireScopeAttr in requireAnyScopeAttrs)
                {
                    var scopes = requireScopeAttr.Arguments?.SelectMany(x => (string[])x);
                    if (scopes.IsNullOrEmpty()) continue;

                    if (scopes.Count() == 1) requireScopes.Add(scopes.First());
                    else requireScopes.Add($"[{scopes.ToString(", ")}]");
                }

                var requireAnyScopeTransformerAttrs = methodInfo.GetCustomAttributes(typeof(RequireAnyScope<>), true).ToList();
                foreach (var requireAnyScopeTransformerAttr in requireAnyScopeTransformerAttrs)
                {
                    var typeFilter = requireAnyScopeTransformerAttr as TypeFilterAttribute;
                    var inputScopes = typeFilter?.Arguments?.SelectMany(x => (string[])x);
                    if (inputScopes.IsNullOrEmpty()) continue;
                    if (Activator.CreateInstance(requireAnyScopeTransformerAttr.GetType().GetGenericArguments()[0]) is not IScopeTransformer transformer)
                        continue;

                    var scopes = inputScopes.Select(inputScope => transformer.Transform(inputScope, controllerDescriptor));
                    if (scopes.Count() == 1) requireScopes.Add(scopes.First());
                    else requireScopes.Add($"[{scopes.ToString(", ")}]");
                }

                var requireIssuerAttrs = methodInfo.GetCustomAttributes<RequireIssuer>(true).ToList();
                requireIssuerAttrs.AddRange(controllerDescriptor.ControllerTypeInfo.GetCustomAttributes<RequireIssuer>(true));
                foreach (var requireScopeAttr in requireIssuerAttrs)
                {
                    var issuer = requireScopeAttr.Arguments?[0].ToString();
                    if (issuer.IsNullOrEmpty()) continue;
                    requireIssuers.Add(issuer);
                }

                var requireAnyIssuerAttrs = methodInfo.GetCustomAttributes<RequireAnyIssuer>(true).ToList();
                requireAnyIssuerAttrs.AddRange(controllerDescriptor.ControllerTypeInfo.GetCustomAttributes<RequireAnyIssuer>(true));
                foreach (var requireIssuerAttr in requireAnyIssuerAttrs)
                {
                    var issuers = requireIssuerAttr.Arguments?.SelectMany(x => (string[])x);
                    if (issuers.IsNullOrEmpty()) continue;

                    if (issuers.Count() == 1) requireIssuers.Add(issuers.First());
                    else requireIssuers.Add($"[{issuers.ToString(", ")}]");
                }

                if (controllerDescriptor.ControllerTypeInfo.IsAssignableToGenericType(typeof(IControllerEndpoint<,>)))
                {
                    var genericArguments = controllerDescriptor.ControllerTypeInfo.GetGenericArguments(typeof(IControllerEndpoint<,>));
                    var controllerType = genericArguments[0];
                    if (controllerType.GetCustomAttribute(genericArguments[1]) is IControllerEndpointAttribute endpointAttribute)
                    {
                        if (endpointAttribute.RequiredAnyScopes.IsNotNullOrEmpty())
                        {
                            if (endpointAttribute.RequiredAnyScopes.Length == 1) requireScopes.Add(endpointAttribute.RequiredAnyScopes.First());
                            else requireScopes.Add($"[{endpointAttribute.RequiredAnyScopes.ToString(", ")}]");
                        }

                        if (endpointAttribute.RequiredAnyIssuers.IsNotNullOrEmpty())
                        {
                            if (endpointAttribute.RequiredAnyIssuers.Length == 1) requireIssuers.Add(endpointAttribute.RequiredAnyIssuers.First());
                            else requireIssuers.Add($"[{endpointAttribute.RequiredAnyIssuers.ToString(", ")}]");
                        }
                    }
                }

                operation.Security.Add(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "AccessToken",
                            },
                        },
                        requireScopes.ToList()
                    }
                });

                if (requireIssuers.IsNotNullOrEmpty())
                {
                    operation.Description = "<b>Required issuers:</b> " +
                        requireIssuers.Select(x => $"`{x}`").ToString(" ") + "<br/>" +
                        operation.Description;
                }
            }

            #endregion

            #region [tag]

            var groupName = baseController.GetCustomAttribute<SwaggerTagGroupAttribute>()?.Name;
            if (groupName.IsNullOrEmpty())
            {
                groupName = baseController.Name;
                if (groupName.EndsWith("Controller")) groupName = groupName[..^10];
            }

            if (operation.Tags.IsNullOrEmpty())
            {
                var tag = new OpenApiTag();
                tag.Name = groupName + tag.Name;
                tag.Extensions.Add("x-tagGroup", new OpenApiString(groupName));
                operation.Tags = new List<OpenApiTag> { tag };
            }
            else
            {
                foreach (var tag in operation.Tags)
                {
                    tag.Name = groupName + tag.Name;
                    tag.Extensions.Add("x-tagGroup", new OpenApiString(groupName));
                }
            }

            #endregion

            operation.OperationId = groupName + controllerDescriptor.ControllerName + controllerDescriptor.ActionName;
            context.SchemaRepository.Schemas.Clear();
        }
    }
}
