using System;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TripleSix.Core.Dto;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.WebApi.Swagger
{
    public class IdentityOperationFilter<TIdentity> : IOperationFilter
        where TIdentity : IIdentity
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var schema = typeof(TIdentity).GenerateSchema(
                context.SchemaGenerator,
                context.SchemaRepository,
                generateDefault: true,
                defaultInstance: Activator.CreateInstance<TIdentity>(),
                excludeProperties: new[] { nameof(IIdentity.HttpContext), nameof(IIdentity.User), nameof(IIdentity.UserId) });

            foreach (var property in schema.Properties)
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    In = ParameterLocation.Header,
                    Name = property.Key,
                    Description = property.Value.Description,
                    Schema = property.Value,
                });
            }

            context.SchemaRepository.Schemas.Clear();
        }
    }
}