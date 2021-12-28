using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CpTech.Core.WebApi.Swagger
{
    public class AuthorizeOperationFilter : IOperationFilter
    {
        private readonly string _authenticationSchemes;
        private readonly Func<ControllerActionDescriptor, string[]> _requirementPredicate;
        private readonly string _securityId;

        public AuthorizeOperationFilter(
            string securityId,
            string authenticationSchemes = null,
            Func<ControllerActionDescriptor, string[]> requirementPredicate = null)
        {
            _securityId = securityId;
            _authenticationSchemes = authenticationSchemes;
            _requirementPredicate = requirementPredicate;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ActionDescriptor is not ControllerActionDescriptor controllerDescriptor)
                return;

            var authorize = (AuthorizeAttribute)context.MethodInfo
                .GetCustomAttributes(typeof(AuthorizeAttribute), true)
                .FirstOrDefault() ?? (AuthorizeAttribute)controllerDescriptor.ControllerTypeInfo
                .GetCustomAttributes(typeof(AuthorizeAttribute), true)
                .FirstOrDefault();

            if (authorize == null)
                return;

            if (!string.IsNullOrWhiteSpace(_authenticationSchemes)
                && authorize.AuthenticationSchemes != _authenticationSchemes)
                return;

            var securityScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = _securityId,
                },
            };
        }
    }
}