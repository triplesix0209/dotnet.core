using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CpTech.Core.WebApi.Swagger
{
    public class BaseDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var (_, openApiPathItem) in swaggerDoc.Paths)
            {
                foreach (var (_, value) in openApiPathItem.Operations)
                {
                    if (value.Summary == null) continue;

                    var tagName = value.Tags[0].Name;
                    var controllerName = swaggerDoc.Tags.FirstOrDefault(x => x.Name == tagName)?.Description
                                         ?? tagName;

                    value.Summary = Regex.Replace(value.Summary, @"\[controller\]", controllerName);
                }
            }

            var orderedPaths = swaggerDoc.Paths.OrderBy(x => x.Key).ToList();
            swaggerDoc.Paths.Clear();
            foreach (var (key, value) in orderedPaths)
                swaggerDoc.Paths.Add(key, value);
        }
    }
}