using Microsoft.OpenApi.Models;
using Sample.Common;
using Swashbuckle.AspNetCore.SwaggerGen;
using TripleSix.Core.WebApi.Swagger;

namespace Sample.WebApi.Swagger
{
    public class IdentityOperationFilter : IdentityOperationFilter<Identity>
    {
        public override void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
        }
    }
}
