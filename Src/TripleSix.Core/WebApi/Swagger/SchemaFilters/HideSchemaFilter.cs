﻿using System;
using System.Linq;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TripleSix.Core.Attributes;

namespace TripleSix.Core.WebApi.Swagger
{
    public class HideSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties.Count == 0) return;
            var hideProperties = context.Type.GetProperties().Where(t => t.GetCustomAttribute<SwaggerHideAttribute>() != null);
            foreach (var hideProperty in hideProperties)
            {
                var propertyToHide = schema.Properties.Keys.SingleOrDefault(x => string.Equals(x, hideProperty.Name, StringComparison.OrdinalIgnoreCase));
                if (propertyToHide != null) schema.Properties.Remove(propertyToHide);
            }
        }
    }
}