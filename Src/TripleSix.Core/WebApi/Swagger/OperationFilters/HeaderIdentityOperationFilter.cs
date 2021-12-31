using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TripleSix.Core.Dto;

namespace TripleSix.Core.WebApi.Swagger
{
    public class HeaderIdentityOperationFilter<TIdentity> : IOperationFilter
        where TIdentity : IIdentity
    {
        public virtual void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ActionDescriptor is not ControllerActionDescriptor controllerDescriptor)
                return;
        }
    }
}