using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TripleSix.Core.WebApi.Swagger
{
    public class DescribeParameterFilter : IParameterFilter
    {
        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            var propertyInfo = context.PropertyInfo;
            if (propertyInfo == null) return;

            var sb = new StringBuilder();

            var attrName = (DisplayNameAttribute)propertyInfo
                .GetCustomAttributes(typeof(DisplayNameAttribute), true)
                .FirstOrDefault();

            var attrDesc = (DescriptionAttribute)propertyInfo
                .GetCustomAttributes(typeof(DescriptionAttribute), true)
                .FirstOrDefault();

            var desc = sb.AppendJoin(attrName?.DisplayName, attrDesc?.Description)
                .ToString();

            if (desc.Length != 0)
                parameter.Description += desc;
        }
    }
}