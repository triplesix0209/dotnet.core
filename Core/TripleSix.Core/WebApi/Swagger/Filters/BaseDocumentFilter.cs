using System.Text.RegularExpressions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TripleSix.Core.WebApi
{
    public class BaseDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var (_, openApiPathItem) in swaggerDoc.Paths)
            {
                foreach (var (_, operation) in openApiPathItem.Operations)
                {
                    if (operation.Summary == null) continue;

                    var tagName = operation.Tags[0].Name;
                    var controllerName = swaggerDoc.Tags.FirstOrDefault(x => x.Name == tagName)?.Description
                                         ?? tagName;

                    operation.Summary = Regex.Replace(operation.Summary, @"\[controller\]", controllerName);
                }
            }

            var orderedPaths = swaggerDoc.Paths.OrderBy(x => x.Key).ToList();
            swaggerDoc.Paths.Clear();
            foreach (var (key, value) in orderedPaths)
                swaggerDoc.Paths.Add(key, value);
        }
    }
}
