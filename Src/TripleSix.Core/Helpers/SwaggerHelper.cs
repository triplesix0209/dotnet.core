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
            bool generateDefault = true,
            object defaultInstance = null,
            OpenApiSchema baseSchema = null,
            PropertyInfo propertyInfo = null,
            string[] excludeProperties = null)
        {
            var result = schemaGenerator.GenerateSchema(type, schemaRepository);
            result.Nullable = type.IsNullableType();
            result.Reference = null;

            var propertyType = type.GetUnderlyingType();
            if (result.Type == "array")
            {
                var elementType = type.IsArray
                    ? type.GetElementType()
                    : type.GetGenericArguments()[0];

                object elementDefaultInstance = null;
                if (generateDefault)
                {
                    elementDefaultInstance = type.IsArray
                        ? (defaultInstance as Array).GetValue(0)
                        : (defaultInstance as IList)[0];
                }

                result.Items.Reference = null;
                result.Items = elementType.GenerateSchema(
                    schemaGenerator,
                    schemaRepository,
                    generateDefault: generateDefault,
                    defaultInstance: elementDefaultInstance,
                    baseSchema: result);
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

                    object propertyDefaultInstance = null;
                    if (generateDefault)
                    {
                        propertyDefaultInstance = property.GetValue(defaultInstance);
                        if (propertyDefaultInstance is null
                            && property.PropertyType.IsClass
                            && property.PropertyType != typeof(string))
                            propertyDefaultInstance = property.PropertyType.CreateDefaultInstance();
                    }

                    result.Properties.Add(
                        property.Name.ToCamelCase(),
                        property.PropertyType.GenerateSchema(
                            schemaGenerator,
                            schemaRepository,
                            generateDefault: generateDefault,
                            defaultInstance: propertyDefaultInstance,
                            baseSchema: result,
                            propertyInfo: property,
                            excludeProperties));
                }
            }
            else if (propertyType.IsEnum)
            {
                result.Reference = null;
                result.Type = "integer";
                result.Format = "int32";
            }

            if (result.Type != "object" && generateDefault)
                result.Default = type.GenerateDefaultValue(defaultInstance);

            if (propertyInfo is null) return result;
            if (propertyInfo.GetCustomAttribute<RequiredValidateAttribute>() is not null
                && result.Default is OpenApiNull
                && baseSchema is not null)
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

        public static IOpenApiAny GenerateDefaultValue(this Type type, object value)
        {
            if (value is null)
                return new OpenApiNull();

            if (value is string)
                return new OpenApiString((string)value);

            if (value is bool)
                return new OpenApiBoolean((bool)value);

            if (value is byte)
                return new OpenApiByte((byte)value);

            if (value is int)
                return new OpenApiInteger((int)value);

            if (value is long)
                return new OpenApiLong((long)value);

            if (value is float)
                return new OpenApiFloat((float)value);

            if (value is double)
                return new OpenApiDouble((double)value);

            if (value is decimal)
                return new OpenApiDouble(Convert.ToDouble(value));

            if (value is DateTime)
                return new OpenApiInteger((int)((DateTime)value).ToEpochTimestamp());

            if (value is Enum)
                return new OpenApiInteger((int)value);

            if (type.IsArray)
            {
                var result = new OpenApiArray();
                var items = value as Array;
                if (items.Length == 0) return result;

                foreach (var item in items)
                    result.Add(type.GetElementType().GenerateDefaultValue(item));

                if (!result.Where(x => x is not null).Any()) return null;
                return result;
            }

            if (typeof(IList).IsAssignableFrom(type))
            {
                var result = new OpenApiArray();
                var items = value as IList;

                var isAny = false;
                foreach (var item in items)
                {
                    isAny = true;
                    result.Add(type.GetGenericArguments()[0].GenerateDefaultValue(item));
                }

                if (isAny && !result.Where(x => x is not null).Any()) return null;
                return result;
            }

            return null;
        }
    }
}