using System;
using System.Linq;
using System.Reflection;
using TripleSix.Core.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TripleSix.Core.WebApi.Swagger
{
    public class HidePropertySchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties.Count == 0) return;
            var hideProperties = context.Type.GetProperties().Where(t => t.GetCustomAttribute<HidePropertyAttribute>() != null);
            foreach (var hideProperty in hideProperties)
            {
                var propertyToHide = schema.Properties.Keys.SingleOrDefault(x => string.Equals(x, hideProperty.Name, StringComparison.OrdinalIgnoreCase));
                if (propertyToHide != null) schema.Properties.Remove(propertyToHide);
            }
        }
    }
}