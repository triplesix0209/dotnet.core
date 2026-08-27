using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json.Serialization;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TripleSix.Core.Helpers;
using TripleSix.Core.Types;
using TripleSix.Core.Validation;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Describe schema filter.
    /// </summary>
    public class DescribeSchemaFilter : ISchemaFilter
    {
        /// <inheritdoc/>
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var type = context.Type;
            var underlyingType = type.GetUnderlyingType();

            if (underlyingType.IsEnum)
            {
                schema.Type = "integer";
                schema.Format = "int32";
                var values = Enum.GetValues(underlyingType).Cast<int>().Select(value =>
                {
                    var name = Enum.GetName(underlyingType, value);
                    var description = EnumHelper.GetDescription(underlyingType, value);
                    return $"<span>{value} = {name} {(name!.Equals(description, StringComparison.CurrentCultureIgnoreCase) ? string.Empty : "(" + description + ")")}</span>";
                });

                if (values.Any())
                    schema.Description = string.Join("<br/>", values);
                return;
            }

            if (underlyingType.IsAssignableTo<System.Text.Json.Nodes.JsonNode>()
                || underlyingType.IsAssignableTo<System.Text.Json.JsonDocument>()
                || underlyingType == typeof(System.Text.Json.JsonElement))
            {
                schema.Type = "object";
                schema.AdditionalProperties = null;
                return;
            }

            if (schema.Properties == null || schema.Properties.Count == 0) return;

            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var propertyName = property.Name.ToCamelCase();
                if (property.GetCustomAttribute<JsonIgnoreAttribute>(true) != null
                    || property.GetCustomAttribute<SwaggerHideAttribute>(true) != null)
                {
                    schema.Properties.Remove(propertyName);
                    continue;
                }

                if (!schema.Properties.TryGetValue(propertyName, out var propertySchema))
                    continue;

                EnrichPropertySchema(propertySchema, property, type, schema);
            }
        }

        private static void EnrichPropertySchema(OpenApiSchema propertySchema, PropertyInfo propertyInfo, Type declaringType, OpenApiSchema baseSchema)
        {
            var propertyType = propertyInfo.PropertyType.GetUnderlyingType();

            if (propertyType.IsEnum)
            {
                propertySchema.Type = "integer";
                propertySchema.Format = "int32";
                var values = Enum.GetValues(propertyType).Cast<int>().Select(value =>
                {
                    var name = Enum.GetName(propertyType, value);
                    var description = EnumHelper.GetDescription(propertyType, value);
                    return $"<span>{value} = {name} {(name!.Equals(description, StringComparison.CurrentCultureIgnoreCase) ? string.Empty : "(" + description + ")")}</span>";
                });

                if (values.Any())
                    propertySchema.Description = (propertySchema.Description.IsNotNullOrEmpty() ? propertySchema.Description + "<br/><br/>" : string.Empty) + string.Join("<br/>", values);
            }

            if (propertyInfo.GetCustomAttribute<RequiredAttribute>() is not null)
            {
                baseSchema.Required ??= new HashSet<string>();
                baseSchema.Required.Add(propertyInfo.Name.ToCamelCase());
                propertySchema.Default = null;
            }

            var displayName = propertyInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName.ToTitleCase();
            var autoDisplayName = displayName == null;
            if (autoDisplayName)
            {
                if (declaringType.IsAssignableToGenericType(typeof(IEntityQueryableDto<>)))
                {
                    displayName ??= declaringType.GetInterfaces()
                        .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEntityQueryableDto<>))?
                        .GenericTypeArguments[0].GetProperty(propertyInfo.Name)?
                        .GetCustomAttribute<CommentAttribute>()?.Comment;
                    if (displayName.IsNotNullOrEmpty())
                        displayName = "Lọc theo " + displayName;
                }
                else
                {
                    var entityType = declaringType.GetGenericArguments(typeof(IMapFromEntityDto<>)).FirstOrDefault() ??
                        declaringType.GetGenericArguments(typeof(IMapToEntityDto<>)).FirstOrDefault();
                    displayName ??= entityType?.GetProperty(propertyInfo.Name)?
                        .GetCustomAttribute<CommentAttribute>()?.Comment;

                    if (displayName.IsNullOrEmpty())
                    {
                        var propertyEntityType = propertyInfo.PropertyType.GetGenericArguments(typeof(IMapFromEntityDto<>)).FirstOrDefault() ??
                            propertyInfo.PropertyType.GetGenericArguments(typeof(IMapToEntityDto<>)).FirstOrDefault();
                        displayName ??= propertyEntityType?.GetCustomAttribute<CommentAttribute>()?.Comment;
                    }
                }
            }

            var description = propertyInfo.GetCustomAttribute<DescriptionAttribute>()?.Description.ToTitleCase();
            var docDescription = new[] { displayName, description }.Where(x => x.IsNotNullOrEmpty()).ToString("<br/>");
            if (docDescription.IsNotNullOrEmpty())
                propertySchema.Description = propertySchema.Description.IsNotNullOrEmpty() ? docDescription + "<br/>" + propertySchema.Description : docDescription;

            propertySchema.MinLength = propertyInfo.GetCustomAttribute<MinLengthAttribute>()?.Length;
            propertySchema.MaxLength = propertyInfo.GetCustomAttribute<MaxLengthAttribute>()?.Length;
            propertySchema.Minimum = propertyInfo.GetCustomAttribute<MinValueAttribute>()?.Value;
            propertySchema.Maximum = propertyInfo.GetCustomAttribute<MaxValueAttribute>()?.Value;

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
                propertySchema.Description = "<span class='sc-laZMeE dmLkmF'>Validators:</span> " +
                    validators.Select(x => x.GetType().Name)
                        .Select(x => x[..^9])
                        .Select(x => x.SplitCase().ToString(" "))
                        .Select(x => $"{x}")
                        .ToString(" ") + "<br/>"
                    + propertySchema.Description;
            }
        }
    }
}
