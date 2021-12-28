using System;
using System.Linq;
using CpTech.Core.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CpTech.Core.WebApi.Swagger
{
    public class HidePropertyOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters.Count > 0)
            {
                var hideParameters = context.ApiDescription.ParameterDescriptions
                    .Where(x => x.CustomAttributes().Any(attr => (Attribute)attr is HidePropertyAttribute));
                foreach (var hideParameter in hideParameters)
                {
                    var propertyToHide = operation.Parameters.SingleOrDefault(x => string.Equals(x.Name, hideParameter.Name, StringComparison.OrdinalIgnoreCase));
                    if (propertyToHide != null) operation.Parameters.Remove(propertyToHide);
                }
            }
        }
    }
}