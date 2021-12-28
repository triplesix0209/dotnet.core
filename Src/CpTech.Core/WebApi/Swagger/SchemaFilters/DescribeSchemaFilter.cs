using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CpTech.Core.WebApi.Swagger
{
    public class DescribeSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var sb = new StringBuilder();

            var attrName = (DisplayNameAttribute)context.MemberInfo?
                .GetCustomAttributes(typeof(DisplayNameAttribute), true)
                .FirstOrDefault();

            var attrDesc = (DescriptionAttribute)context.MemberInfo?
                .GetCustomAttributes(typeof(DescriptionAttribute), true)
                .FirstOrDefault();

            var desc = sb.AppendJoin("<br/>", attrName?.DisplayName, attrDesc?.Description)
                .ToString();

            if (desc != "<br/>")
                schema.Description += desc;
        }
    }
}