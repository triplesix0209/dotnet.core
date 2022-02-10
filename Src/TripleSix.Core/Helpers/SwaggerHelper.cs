using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using TripleSix.Core.Attributes;

namespace TripleSix.Core.Helpers
{
    public static class SwaggerHelper
    {
        public static OpenApiSchema GenerateSchema(
            this Type type,
            ISchemaGenerator schemaGenerator,
            SchemaRepository schemaRepository,
            object defaultInstance = null,
            OpenApiSchema baseSchema = null,
            PropertyInfo propertyInfo = null,
            string[] excludeProperties = null)
        {
            var result = schemaGenerator.GenerateSchema(type, schemaRepository);
            var propertyType = Nullable.GetUnderlyingType(type) is not null ? Nullable.GetUnderlyingType(type) : type;
            result.Nullable = Nullable.GetUnderlyingType(type) is not null || type.IsClass;
            result.Reference = null;

            if (propertyType.IsEnum)
            {
                result.Reference = null;
                result.Type = "integer";
                result.Format = "int32";
            }
            else if (result.Type is null)
            {
                result.Type = "object";
                var properties = type.GetProperties()
                    .OrderBy(x =>
                    {
                        var jsonProperty = x.GetCustomAttributes(typeof(JsonPropertyAttribute), true)
                            .FirstOrDefault() as JsonPropertyAttribute;
                        return jsonProperty?.Order ?? 0;
                    });
                foreach (var property in properties)
                {
                    if (property.GetCustomAttributes(typeof(JsonIgnoreAttribute), true).Any()
                        || property.GetCustomAttributes(typeof(SwaggerHideAttribute), true).Any()
                        || (excludeProperties is not null && excludeProperties.Contains(property.Name)))
                        continue;

                    var defaultValue = defaultInstance is null ? null : property.GetValue(defaultInstance);
                    if (defaultValue is null && property.PropertyType != typeof(string))
                        defaultValue = Activator.CreateInstance(property.PropertyType);

                    result.Properties.Add(
                        property.Name.ToCamelCase(),
                        property.PropertyType.GenerateSchema(
                            schemaGenerator,
                            schemaRepository,
                            defaultValue,
                            result,
                            property,
                            excludeProperties));
                }
            }
            else
            {
                result.Default = type.DefaultValue(defaultInstance);
            }

            if (propertyInfo is null) return result;

            if (propertyInfo.GetCustomAttribute<RequiredValidateAttribute>() is not null
                && defaultInstance is null)
                baseSchema.Required.Add(propertyInfo.Name.ToCamelCase());

            var displayName = propertyInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
            var description = propertyInfo.GetCustomAttribute<DescriptionAttribute>()?.Description;
            result.Description = string.Join("<br/>", new[] { displayName, description }.Where(x => x.IsNotNullOrWhiteSpace()));

            result.Pattern = propertyInfo.GetCustomAttribute<RegexValidateAttribute>()?.Pattern;
            result.MinLength = propertyInfo.GetCustomAttribute<StringLengthValidateAttribute>()?.MinimumLength;
            result.MaxLength = propertyInfo.GetCustomAttribute<StringLengthValidateAttribute>()?.MaximumLength;
            if (propertyInfo.GetCustomAttribute<RangeValidateAttribute>() is not null)
            {
                result.Minimum = propertyInfo.GetCustomAttribute<RangeValidateAttribute>()?.Minimum as decimal?;
                result.Maximum = propertyInfo.GetCustomAttribute<RangeValidateAttribute>()?.Maximum as decimal?;
            }
            else
            {
                result.Minimum = propertyInfo.GetCustomAttribute<MinValidateAttribute>()?.MinValue;
                result.Maximum = propertyInfo.GetCustomAttribute<MaxValidateAttribute>()?.MaxValue;
            }

            if (propertyType.IsEnum)
            {
                var values = EnumHelper.GetValues(propertyType).Cast<int>()
                    .Select(value => $"<span>{value} = {EnumHelper.GetDescription(propertyType, value)}</span>");
                if (values.Any())
                    result.Description += "<br/><br/>" + string.Join("<br/>", values);
            }

            return result;
        }

        public static IOpenApiAny DefaultValue(this Type type, object defaultValue)
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

            if (defaultValue is decimal)
                return new OpenApiDouble(Convert.ToDouble(defaultValue));

            if (defaultValue is double)
                return new OpenApiDouble((double)defaultValue);

            if (defaultValue is DateTime)
                return new OpenApiInteger((int)((DateTime)defaultValue).ToEpochTimestamp());

            if (defaultValue is Enum)
                return new OpenApiInteger((int)defaultValue);

            if (type.IsArray)
            {
                var result = new OpenApiArray();
                var items = defaultValue as Array;
                if (items.Length == 0) return result;

                foreach (var item in items)
                    result.Add(type.GetElementType().DefaultValue(item));

                if (result.Count == 0) return null;
                return result;
            }

            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                var result = new OpenApiArray();
                var items = defaultValue as IEnumerable;

                var isAny = false;
                foreach (var item in items)
                {
                    isAny = true;
                    result.Add(type.GetElementType().DefaultValue(item));
                }

                if (isAny && result.Count == 0) return null;
                return result;
            }

            return null;
        }
    }
}