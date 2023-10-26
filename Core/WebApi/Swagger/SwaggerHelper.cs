using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using TripleSix.Core.Helpers;
using TripleSix.Core.Mappers;
using TripleSix.Core.Types;
using TripleSix.Core.Validation;

namespace TripleSix.Core.WebApi
{
    public static class SwaggerHelper
    {
        private const string FilterDtoPostfix = "FilterDto";

        public static OpenApiSchema GenerateSwaggerSchema(
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

            if (propertyType.IsEnum)
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
            result.Nullable = objectType.IsNullableType();
            if (result.Type != "object" && generateDefaultValue)
                result.Default = objectType.SwaggerValue(defaultValue);

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
                displayName ??= propertyInfo.DeclaringType?
                    .GetCustomAttribute<MapFromEntityAttribute>()?
                    .EntityType.GetProperty(propertyInfo.Name)?
                    .GetCustomAttribute<CommentAttribute>()?.Comment;
                displayName ??= propertyInfo.DeclaringType?
                    .GetCustomAttribute<MapToEntityAttribute>()?
                    .EntityType.GetProperty(propertyInfo.Name)?
                    .GetCustomAttribute<CommentAttribute>()?.Comment;
                displayName ??= propertyInfo.DeclaringType?
                    .GetRawGeneric(typeof(BaseQueryDto<>))?
                    .GenericTypeArguments[0].GetProperty(propertyInfo.Name)?
                    .GetCustomAttribute<CommentAttribute>()?.Comment;
                displayName ??= propertyInfo.DeclaringType?
                    .GetCustomAttribute<MapFromEntityAttribute>()?
                    .EntityType.GetCustomAttribute<CommentAttribute>()?.Comment;

                if (!displayName.IsNullOrEmpty())
                    if (propertyInfo.DeclaringType?.Name.EndsWith(FilterDtoPostfix) == true) displayName = "Lọc theo " + displayName;
            }

            var description = propertyInfo.GetCustomAttribute<DescriptionAttribute>()?.Description.ToTitleCase();
            result.Description = new[] { displayName, description }.Where(x => !x.IsNullOrEmpty()).ToString("<br/>")
                + result.Description;

            result.MinLength = propertyInfo.GetCustomAttribute<MinLengthAttribute>()?.Length;
            result.MaxLength = propertyInfo.GetCustomAttribute<MaxLengthAttribute>()?.Length;
            result.Minimum = propertyInfo.GetCustomAttribute<MinValueAttribute>()?.Value;
            result.Maximum = propertyInfo.GetCustomAttribute<MaxValueAttribute>()?.Value;

            if (parentPropertyInfo != null && parentPropertyInfo.PropertyType.IsAssignableTo<IFilterParameter>())
            {
                var parameterDisplayName = parentPropertyInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
                var parameterName = parameterDisplayName != null && parameterDisplayName.StartsWith("lọc theo ", StringComparison.CurrentCultureIgnoreCase)
                    ? parameterDisplayName[9..]
                    : parameterDisplayName;

                result.Description = result.Description.Replace(
                    $"[{nameof(parameterDisplayName).ToKebabCase()}]",
                    parameterDisplayName);
                result.Description = result.Description.Replace(
                    $"[{nameof(parameterName).ToKebabCase()}]",
                    parameterName);
            }

            if (result.Nullable && propertyInfo.GetCustomAttributes().Any(x => x.GetType().FullName == "System.Runtime.CompilerServices.NullableAttribute"))
                result.Nullable = false;

            var validators = new List<Attribute>();
            var requireAttr = propertyInfo.GetCustomAttribute<RequiredAttribute>();
            var notEmptyAttr = propertyInfo.GetCustomAttribute<NotEmptyAttribute>();
            if ((requireAttr != null && !requireAttr.AllowEmptyStrings) || notEmptyAttr != null)
                validators.Add(new NotEmptyAttribute());

            var notNullAtrr = propertyInfo.GetCustomAttribute<NotNullAttribute>();
            if (notNullAtrr != null) validators.Add(notNullAtrr);

            var noSpaceAtrr = propertyInfo.GetCustomAttribute<NoSpaceAttribute>();
            if (noSpaceAtrr != null) validators.Add(noSpaceAtrr);

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

        public static IOpenApiAny? SwaggerValue(this Type type, object? value)
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
