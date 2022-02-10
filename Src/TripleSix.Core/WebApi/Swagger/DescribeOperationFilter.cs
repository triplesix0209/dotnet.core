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
using TripleSix.Core.Helpers;
using TripleSix.Core.WebApi.Results;

namespace TripleSix.Core.WebApi.Swagger
{
    public class DescribeOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var methodInfo = context.MethodInfo;
            if (methodInfo is null) return;
            var info = methodInfo
                .GetCustomAttributes(typeof(SwaggerApiAttribute), true)
                .FirstOrDefault() as SwaggerApiAttribute;

            operation.Summary = info?.Summary;
            operation.Description = info?.Description;

            var successResponse = new OpenApiResponse { Description = "Success" };
            var dataInstance = info.ResponseType.GenericTypeArguments.Length == 1
                ? Activator.CreateInstance(info.ResponseType.GenericTypeArguments[0])
                : null;
            var defaultInstance = dataInstance is null
                ? Activator.CreateInstance(info.ResponseType)
                : Activator.CreateInstance(info.ResponseType, new[] { dataInstance });
            successResponse.Content.Add("application/json", new OpenApiMediaType
            {
                Schema = info is null
                    ? GenerateSchema(typeof(SuccessResult), context)
                    : GenerateSchema(info.ResponseType, context, defaultInstance),
            });
            operation.Responses["200"] = successResponse;

            var errorResponse = new OpenApiResponse { Description = "Error" };
            errorResponse.Content.Add("application/json", new OpenApiMediaType
            {
                Schema = GenerateSchema(typeof(ErrorResult), context),
            });
            operation.Responses["500"] = errorResponse;

            context.SchemaRepository.Schemas.Clear();
        }

        private static OpenApiSchema GenerateSchema(
            Type type,
            OperationFilterContext context,
            object defaultInstance = null,
            OpenApiSchema baseSchema = null,
            PropertyInfo propertyInfo = null)
        {
            var result = context.SchemaGenerator.GenerateSchema(type, context.SchemaRepository);
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
                        || property.GetCustomAttributes(typeof(SwaggerHideAttribute), true).Any())
                        continue;

                    var defaultValue = defaultInstance is null ? null : property.GetValue(defaultInstance);
                    if (defaultValue is null && property.PropertyType != typeof(string))
                        defaultValue = Activator.CreateInstance(property.PropertyType);

                    result.Properties.Add(
                        property.Name.ToCamelCase(),
                        GenerateSchema(
                            property.PropertyType,
                            context,
                            defaultValue,
                            result,
                            property));
                }
            }
            else
            {
                result.Default = DefaultValue(defaultInstance, type);
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

        private static IOpenApiAny DefaultValue(object defaultValue, Type type)
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
                    result.Add(DefaultValue(item, type.GetElementType()));

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
                    result.Add(DefaultValue(item, type.GetElementType()));
                }

                if (isAny && result.Count == 0) return null;
                return result;
            }

            return null;
        }
    }
}