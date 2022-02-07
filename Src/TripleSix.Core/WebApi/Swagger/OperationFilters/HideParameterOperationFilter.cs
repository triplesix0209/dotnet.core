using System;
using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TripleSix.Core.Attributes;

namespace TripleSix.Core.WebApi.Swagger
{
    public class HideParameterOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters.Count > 0)
            {
                var hideParameters = context.ApiDescription.ParameterDescriptions
                    .Where(x => x.CustomAttributes().Any(attr => (Attribute)attr is SwaggerHideAttribute));
                foreach (var hideParameter in hideParameters)
                {
                    var propertyToHide = operation.Parameters.SingleOrDefault(x => string.Equals(x.Name, hideParameter.Name, StringComparison.OrdinalIgnoreCase));
                    if (propertyToHide != null) operation.Parameters.Remove(propertyToHide);
                }
            }
        }
    }
}