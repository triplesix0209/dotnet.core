using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.SwaggerGen;
using TripleSix.Core.Helpers;
using TripleSix.Core.Mappers;
using TripleSix.Core.Types;
using TripleSix.Core.Validation;

namespace TripleSix.Core.WebApi
{
    internal static class SwaggerHelper
    {
        internal static OpenApiSchema GenerateSwaggerSchema(
            this Type objectType,
            ISchemaGenerator schemaGenerator,
            SchemaRepository schemaRepository,
            PropertyInfo? propertyInfo = null,
            PropertyInfo? parentPropertyInfo = null,
            object? defaultValue = null,
            OpenApiSchema? baseSchema = null,
            bool generateDefaultValue = true)
        {
            var result = schemaGenerator.GenerateSchema(objectType, schemaRepository);
            var propertyType = objectType.GetUnderlyingType();

            if (propertyType.IsAssignableTo<JToken>())
            {
                result.Type = "object";
                result.AdditionalProperties = null;
            }
            else if (propertyType.IsAssignableTo<IFormFile>())
            {
                //result.Type = "string";
                //result.Format = "binary";
            }
            else if (propertyType.IsEnum)
            {
                result.Type = "integer";
                result.Format = "int32";
            }
            else if (result.Type == "array")
            {
                var elementType = objectType.IsArray
                   ? objectType.GetElementType()
                   : objectType.GetGenericArguments()[0];

                result.Items.Reference = null;
                result.Items = elementType == null
                    ? null
                    : elementType.GenerateSwaggerSchema(
                        schemaGenerator,
                        schemaRepository,
                        defaultValue: defaultValue,
                        baseSchema: result,
                        generateDefaultValue: generateDefaultValue);
            }
            else if (result.Type is null)
            {
                result.Type = "object";
                var properties = objectType.GetProperties()
                    .OrderBy(x => x.DeclaringType?.BaseTypesAndSelf().Count())
                    .OrderBy(x => x.GetCustomAttribute<JsonPropertyAttribute>(true)?.Order ?? 0);

                foreach (var property in properties)
                {
                    if (property.GetCustomAttribute<JsonIgnoreAttribute>(true) != null
                        || property.GetCustomAttribute<SwaggerHideAttribute>(true) != null)
                        continue;

                    result.Properties.Add(
                        property.Name.ToCamelCase(),
                        property.PropertyType.GenerateSwaggerSchema(
                            schemaGenerator,
                            schemaRepository,
                            propertyInfo: property,
                            parentPropertyInfo: propertyInfo,
                            defaultValue: defaultValue == null ? null : property.GetValue(defaultValue),
                            baseSchema: result,
                            generateDefaultValue: generateDefaultValue));
                }
            }

            result.Reference = null;
            if (result.Type != "object" && generateDefaultValue) result.Default = objectType.SwaggerValue(defaultValue);
            result.Nullable = objectType.IsNullableType();

            if (propertyType.IsEnum)
            {
                var values = Enum.GetValues(propertyType).Cast<int>().Select(value =>
                {
                    var name = Enum.GetName(propertyType, value);
                    var description = EnumHelper.GetDescription(propertyType, value);
                    return $"<span>{value} = {name} {(name!.Equals(description, StringComparison.CurrentCultureIgnoreCase) ? string.Empty : "(" + description + ")")}</span>";
                });

                if (values.Any())
                    result.Description += "<br/><br/>" + string.Join("<br/>", values);
            }

            if (propertyInfo == null) return result;
            if (propertyInfo.GetCustomAttribute<RequiredAttribute>() is not null && baseSchema is not null)
            {
                baseSchema.Required.Add(propertyInfo.Name.ToCamelCase());
                result.Default = null;
            }

            var displayName = propertyInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName.ToTitleCase();
            var autoDisplayName = displayName == null;
            if (autoDisplayName)
            {
                if (propertyInfo.DeclaringType?.IsAssignableToGenericType(typeof(IEntityQueryableDto<>)) == true)
                {
                    displayName ??= propertyInfo.DeclaringType.GetInterfaces()
                        .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEntityQueryableDto<>))?
                        .GenericTypeArguments[0].GetProperty(propertyInfo.Name)?
                        .GetCustomAttribute<CommentAttribute>()?.Comment;
                    if (displayName.IsNotNullOrEmpty())
                        displayName = "Lọc theo " + displayName;
                }
                else if (propertyInfo.DeclaringType?.IsAssignableToGenericType(typeof(IElasticQueryableDto<>)) == true)
                {
                    var documentType = propertyInfo.DeclaringType.GetInterfaces()
                        .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IElasticQueryableDto<>))?
                        .GenericTypeArguments[0];
                    var entityType = documentType?
                        .GetCustomAttribute(typeof(MapFromEntityAttribute<>))?
                        .GetType().GetGenericArguments()[0];
                    displayName ??= entityType?.GetProperty(propertyInfo.Name)?
                        .GetCustomAttribute<CommentAttribute>()?.Comment;
                    if (displayName.IsNotNullOrEmpty())
                        displayName = "Lọc theo " + displayName;
                }
                else
                {
                    var entityType = propertyInfo.DeclaringType?
                        .GetCustomAttribute(typeof(MapFromEntityAttribute<>))?
                        .GetType().GetGenericArguments()[0] ??
                        propertyInfo.DeclaringType?
                        .GetCustomAttribute(typeof(MapToEntityAttribute<>))?
                        .GetType().GetGenericArguments()[0];
                    if (entityType == null)
                    {
                        var documentType = propertyInfo.DeclaringType?
                        .GetCustomAttribute(typeof(MapFromElasticDocumentAttribute<>))?
                        .GetType().GetGenericArguments()[0];
                        entityType = documentType?
                            .GetCustomAttribute(typeof(MapFromEntityAttribute<>))?
                            .GetType().GetGenericArguments()[0];
                    }

                    displayName ??= entityType?.GetProperty(propertyInfo.Name)?
                        .GetCustomAttribute<CommentAttribute>()?.Comment;

                    if (displayName.IsNullOrEmpty())
                    {
                        var propertyEntityType = propertyInfo.PropertyType
                            .GetCustomAttribute(typeof(MapFromEntityAttribute<>))?
                            .GetType().GetGenericArguments()[0] ??
                            propertyInfo.PropertyType
                            .GetCustomAttribute(typeof(MapToEntityAttribute<>))?
                            .GetType().GetGenericArguments()[0];
                        displayName ??= propertyEntityType?.GetCustomAttribute<CommentAttribute>()?.Comment;
                    }
                }
            }

            var description = propertyInfo.GetCustomAttribute<DescriptionAttribute>()?.Description.ToTitleCase();
            result.Description = new[] { displayName, description }.Where(x => x.IsNotNullOrEmpty()).ToString("<br/>")
                + result.Description;

            result.MinLength = propertyInfo.GetCustomAttribute<MinLengthAttribute>()?.Length;
            result.MaxLength = propertyInfo.GetCustomAttribute<MaxLengthAttribute>()?.Length;
            result.Minimum = propertyInfo.GetCustomAttribute<MinValueAttribute>()?.Value;
            result.Maximum = propertyInfo.GetCustomAttribute<MaxValueAttribute>()?.Value;

            var validators = new List<Attribute>();
            var requireAttr = propertyInfo.GetCustomAttribute<RequiredAttribute>();
            var notEmptyAttr = propertyInfo.GetCustomAttribute<NotEmptyAttribute>();
            if ((requireAttr != null && !requireAttr.AllowEmptyStrings) || notEmptyAttr != null)
                validators.Add(new NotEmptyAttribute());

            var notNullAtrr = propertyInfo.GetCustomAttribute<NotNullAttribute>();
            if (notNullAtrr != null) validators.Add(notNullAtrr);

            var mustNoSpaceAtrr = propertyInfo.GetCustomAttribute<MustNoSpaceAttribute>();
            if (mustNoSpaceAtrr != null) validators.Add(mustNoSpaceAtrr);

            var mustLowerCaseAtrr = propertyInfo.GetCustomAttribute<MustLowerCaseAttribute>();
            if (mustLowerCaseAtrr != null) validators.Add(mustLowerCaseAtrr);

            var mustUpperCaseAtrr = propertyInfo.GetCustomAttribute<MustUpperCaseAttribute>();
            if (mustUpperCaseAtrr != null) validators.Add(mustUpperCaseAtrr);

            var mustTrimAtrr = propertyInfo.GetCustomAttribute<MustTrimAttribute>();
            if (mustTrimAtrr != null) validators.Add(mustTrimAtrr);

            var mustWordNumberAtrr = propertyInfo.GetCustomAttribute<MustWordNumberAttribute>();
            if (mustWordNumberAtrr != null) validators.Add(mustWordNumberAtrr);

            var mustNumberAtrr = propertyInfo.GetCustomAttribute<MustNumberAttribute>();
            if (mustNumberAtrr != null) validators.Add(mustNumberAtrr);

            var mustEmailAtrr = propertyInfo.GetCustomAttribute<MustEmailAttribute>();
            if (mustEmailAtrr != null) validators.Add(mustEmailAtrr);

            var mustPhoneAtrr = propertyInfo.GetCustomAttribute<MustPhoneAttribute>();
            if (mustPhoneAtrr != null) validators.Add(mustPhoneAtrr);

            var mustRegExrAtrr = propertyInfo.GetCustomAttribute<MustRegExrAttribute>();
            if (mustRegExrAtrr != null) validators.Add(mustRegExrAtrr);

            if (validators.Any())
            {
                result.Description = "<span class='sc-laZMeE dmLkmF'>Validators:</span> " +
                    validators.Select(x => x.GetType().Name)
                        .Select(x => x[..^9])
                        .Select(x => x.SplitCase().ToString(" "))
                        .Select(x => $"`{x}`")
                        .ToString(" ") + "<br/>"
                    + result.Description;
            }

            return result;
        }

        internal static IOpenApiAny? SwaggerValue(this Type type, object? value)
        {
            if (value is null)
                return new OpenApiNull();

            if (value is string str)
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
                if (value is not Array items) return result;
                if (items.Length == 0) return result;

                foreach (var item in items)
                    result.Add(item.GetType().SwaggerValue(item));

                if (!result.Where(x => x is not null).Any()) return null;
                return result;
            }

            if (type.IsAssignableTo<ICollection>())
            {
                var result = new OpenApiArray();
                if (value is not ICollection items) return result;
                if (items.Count == 0) return result;

                foreach (var item in items)
                    result.Add(item.GetType().SwaggerValue(item));

                if (!result.Where(x => x is not null).Any()) return null;
                return result;
            }

            return null;
        }
    }
}
