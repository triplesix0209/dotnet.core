using System.ComponentModel;
using System.Linq;
using System.Text;
using TripleSix.Core.Helpers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TripleSix.Core.WebApi.Swagger
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (!context.Type.IsEnum) return;

            var enumDesc = (DescriptionAttribute)context.Type
                .GetCustomAttributes(typeof(DescriptionAttribute), true)
                .FirstOrDefault();

            var sb = new StringBuilder(enumDesc?.Description ?? string.Empty);
            var values = EnumHelper.GetValues(context.Type).Cast<int>();
            foreach (var value in values)
            {
                sb.Append("<br/>")
                    .Append(value)
                    .Append(" = ")
                    .Append(EnumHelper.GetDescription(context.Type, value));
            }

            schema.Description += sb.ToString();
        }
    }
}