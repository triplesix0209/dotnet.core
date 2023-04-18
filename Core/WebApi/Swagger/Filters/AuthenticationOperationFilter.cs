using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.WebApi
{
    public class AuthenticationOperationFilter : IOperationFilter
    {
        private string _defaultSecurityScheme;

        public AuthenticationOperationFilter(string defaultSecurityScheme)
        {
            _defaultSecurityScheme = defaultSecurityScheme;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ActionDescriptor is not ControllerActionDescriptor controllerInfo) return;
            var methodInfo = controllerInfo.MethodInfo;
            if (methodInfo == null) return;

            if (controllerInfo.EndpointMetadata.FirstOrDefault(x => x.GetType() == typeof(AuthorizeAttribute)) is not AuthorizeAttribute authorize) return;

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
    }
}
