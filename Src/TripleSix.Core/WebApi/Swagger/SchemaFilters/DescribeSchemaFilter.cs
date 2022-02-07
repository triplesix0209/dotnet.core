using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TripleSix.Core.Attributes;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.WebApi.Swagger
{
    public class DescribeSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var propertyType = Nullable.GetUnderlyingType(context.Type) ?? context.Type;

            if (propertyType.IsEnum)
            {
                schema.Reference = null;
                schema.Type = "integer";
                schema.Format = "int32";
                schema.Enum.Clear();

                var enumDesc = propertyType.GetCustomAttribute<DescriptionAttribute>();
                if (enumDesc is not null)
                    schema.Description += enumDesc.Description + "<br/>";

                var values = EnumHelper.GetValues(propertyType).Cast<int>()
                    .Select(value => $"<span class=\"sc-fzqzlV sc-fzonjX iwWBsD\">{value} = {EnumHelper.GetDescription(propertyType, value)}</span>");
                if (values.Any())
                {
                    schema.Description += "<br/>"
                        + "<span class=\"sc-fzqzlV jDREaU\"> Values: </span><br/>"
                        + string.Join("<br/>", values);
                }
            }

            var propertyInfo = context.MemberInfo as PropertyInfo;
            if (propertyInfo is null) return;
            if (!propertyInfo.DeclaringType.IsGenericType)
            {
                var defaultValue = propertyInfo.GetValue(Activator.CreateInstance(propertyInfo.DeclaringType));
                if (schema.Default is null)
                    schema.Default = SwaggerHelper.DefaultValue(defaultValue, propertyType);
            }

            var attrName = (DisplayNameAttribute)propertyInfo
                .GetCustomAttributes(typeof(DisplayNameAttribute), true)
                .FirstOrDefault();
            var attrDesc = (DescriptionAttribute)propertyInfo
                .GetCustomAttributes(typeof(DescriptionAttribute), true)
                .FirstOrDefault();
            var description = string.Join("<br/>", attrName?.DisplayName, attrDesc?.Description);
            if (description.IsNotNullOrWhiteSpace()) schema.Description += description;

            var minValue = ((MinValidateAttribute)propertyInfo.GetCustomAttributes(typeof(MinValidateAttribute), true)
                .FirstOrDefault())
                ?.MinValue;
            if (minValue.HasValue) schema.Minimum = minValue;

            var maxValue = ((MaxValidateAttribute)propertyInfo.GetCustomAttributes(typeof(MaxValidateAttribute), true)
                .FirstOrDefault())
                ?.MaxValue;
            if (maxValue.HasValue) schema.Maximum = maxValue;
        }
    }
}