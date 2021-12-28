using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CpTech.Core.Helpers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CpTech.Core.WebApi.Swagger
{
    public class EnumParameterFilter : IParameterFilter
    {
        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            var propertyInfo = context.PropertyInfo;
            if (propertyInfo == null) return;

            var type = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
            if (!type.IsEnum) return;

            var enumDesc = (DescriptionAttribute)type
                .GetCustomAttributes(typeof(DescriptionAttribute), true)
                .FirstOrDefault();

            var sb = new StringBuilder(enumDesc?.Description ?? string.Empty);
            var values = EnumHelper.GetValues(type).Cast<int>();
            foreach (var value in values)
            {
                sb.Append("<br/>")
                    .Append(value)
                    .Append(" = ")
                    .Append(EnumHelper.GetDescription(type, value));
            }

            parameter.Description += sb.ToString();
        }
    }
}