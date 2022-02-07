using System;
using Microsoft.OpenApi.Any;

namespace TripleSix.Core.Helpers
{
    public static class SwaggerHelper
    {
        public static IOpenApiAny DefaultValue(object defaultValue, Type type)
        {
            if (defaultValue is null)
                return new OpenApiNull();

            if (defaultValue is string)
                return new OpenApiString((string)defaultValue);

            if (defaultValue is bool)
                return new OpenApiBoolean((bool)defaultValue);

            if (defaultValue is byte)
                return new OpenApiByte((byte)defaultValue);

            if (defaultValue is int)
                return new OpenApiInteger((int)defaultValue);

            if (defaultValue is long)
                return new OpenApiLong((long)defaultValue);

            if (defaultValue is float)
                return new OpenApiFloat((float)defaultValue);

            if (defaultValue is double)
                return new OpenApiDouble((double)defaultValue);

            if (defaultValue is Enum)
                return new OpenApiInteger((int)defaultValue);

            if (type.IsArray)
            {
                var result = new OpenApiArray();
                var items = defaultValue as Array;
                if (items.Length == 0) return result;

                foreach (var item in items)
                    result.Add(DefaultValue(item, type.GetElementType()));

                if (result.Count == 0) return null;
                return result;
            }

            return null;
        }
    }
}
