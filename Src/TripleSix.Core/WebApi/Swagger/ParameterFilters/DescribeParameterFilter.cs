using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TripleSix.Core.Attributes;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.WebApi.Swagger
{
    public class DescribeParameterFilter : IParameterFilter
    {
        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            var propertyInfo = context.PropertyInfo;
            if (propertyInfo is null) return;
            var defaultValue = propertyInfo.GetValue(Activator.CreateInstance(propertyInfo.DeclaringType));
            var propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;

            if (parameter.Schema.Default is null)
                parameter.Schema.Default = SwaggerHelper.DefaultValue(defaultValue, propertyType);

            var attrName = (DisplayNameAttribute)propertyInfo
                .GetCustomAttributes(typeof(DisplayNameAttribute), true)
                .FirstOrDefault();
            var attrDesc = (DescriptionAttribute)propertyInfo
                .GetCustomAttributes(typeof(DescriptionAttribute), true)
                .FirstOrDefault();
            var description = string.Join("<br/>", attrName?.DisplayName, attrDesc?.Description);
            if (description.IsNotNullOrWhiteSpace()) parameter.Description += description;

            parameter.Required = propertyInfo.GetCustomAttributes(typeof(RequiredValidateAttribute), true).Any()
                && defaultValue is null;

            var minValue = ((MinValidateAttribute)propertyInfo.GetCustomAttributes(typeof(MinValidateAttribute), true)
                .FirstOrDefault())
                ?.MinValue;
            if (minValue.HasValue) parameter.Schema.Minimum = minValue;

            var maxValue = ((MaxValidateAttribute)propertyInfo.GetCustomAttributes(typeof(MaxValidateAttribute), true)
                .FirstOrDefault())
                ?.MaxValue;
            if (maxValue.HasValue) parameter.Schema.Maximum = maxValue;

            if (propertyType.IsEnum)
            {
                parameter.Schema.Reference = null;
                parameter.Schema.Type = "integer";
                parameter.Schema.Format = "int32";

                var values = EnumHelper.GetValues(propertyType).Cast<int>()
                    .Select(value => $"<span class=\"sc-fzqzlV sc-fzonjX iwWBsD\">{value} = {EnumHelper.GetDescription(propertyType, value)}</span>");
                if (values.Any())
                {
                    parameter.Description += "<br/>"
                        + "<span class=\"sc-fzqzlV jDREaU\"> Values: </span><br/>"
                        + string.Join("<br/>", values);
                }
            }
        }
    }
}