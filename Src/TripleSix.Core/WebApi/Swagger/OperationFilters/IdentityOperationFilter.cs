using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TripleSix.Core.Dto;

namespace TripleSix.Core.WebApi.Swagger
{
    public abstract class IdentityOperationFilter<TIdentity> : IOperationFilter
        where TIdentity : IIdentity
    {
        public abstract void Apply(OpenApiOperation operation, OperationFilterContext context);
    }
}