using System.Reflection;
using System.Text.RegularExpressions;
using Elastic.Transport.Extensions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Base document filter.
    /// </summary>
    public class BaseDocumentFilter : IDocumentFilter
    {
        /// <inheritdoc/>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            // prepare tag group
            var tagGroups = new List<TagGroupItem>();
            foreach (var (apiPath, openApiPathItem) in swaggerDoc.Paths)
            {
                foreach (var (apiMethod, operation) in openApiPathItem.Operations)
                {
                    var apiDescription = context.ApiDescriptions.First(x => x.HttpMethod == apiMethod.GetStringValue().ToUpper() && x.RelativePath == apiPath[1..]);
                    if (apiDescription.ActionDescriptor is not ControllerActionDescriptor controllerDescriptor) continue;
                    var controllerType = controllerDescriptor.ControllerTypeInfo;
                    var baseControllerType = controllerType.BaseType;
                    if (baseControllerType is null) continue;

                    var swaggerTagGroupAttr = baseControllerType.GetCustomAttribute<SwaggerTagGroupAttribute>();
                    var swaggerTagAttr = controllerType.GetCustomAttribute<SwaggerTagAttribute>();
                    if (swaggerTagAttr is null && controllerType.IsAssignableToGenericType(typeof(IControllerEndpoint<,>)))
                        swaggerTagAttr = controllerType.GetGenericArguments(typeof(IControllerEndpoint<,>))[0].GetCustomAttribute<SwaggerTagAttribute>();

                    foreach (var tag in operation.Tags)
                    {
                        var groupName = ((OpenApiString)tag.Extensions.First(x => x.Key == "x-tagGroup").Value).Value;
                        var tagGroup = tagGroups.FirstOrDefault(x => x.Name == groupName);

                        if (tagGroup == null)
                        {
                            tagGroup = new TagGroupItem
                            {
                                Name = groupName,
                                OrderIndex = swaggerTagGroupAttr?.OrderIndex ?? 0,
                                Description = swaggerTagGroupAttr?.Description,
                                Tags = new List<TagItem>(),
                            };
                            tagGroups.Add(tagGroup);
                        }

                        if (!tagGroup.Tags.Any(x => x.Name == tag.Name))
                        {
                            tagGroup.Tags.Add(new TagItem
                            {
                                Name = tag.Name,
                                Description = swaggerTagAttr?.Description,
                            });
                        }
                    }
                }
            }

            // build swagger tags
            swaggerDoc.Tags.Clear();
            foreach (var tag in tagGroups.SelectMany(x => x.Tags))
            {
                swaggerDoc.Tags.Add(new OpenApiTag
                {
                    Name = tag.Name,
                    Description = tag.Description?.ToTitleCase() ?? null,
                    Extensions = new Dictionary<string, IOpenApiExtension>()
                    {
                        { "x-displayName", new OpenApiString(tag.Description ?? tag.Name) },
                    },
                });
            }

            // build swagger tag groups
            var xTagGroups = new OpenApiArray();
            swaggerDoc.Extensions.Add("x-tagGroups", xTagGroups);
            foreach (var group in tagGroups.OrderBy(x => x.OrderIndex))
            {
                var xTagGroupItem = new OpenApiArray();
                xTagGroupItem.AddRange(group.Tags.Select(x => new OpenApiString(x.Name)));
                xTagGroups.Add(new OpenApiObject()
                {
                    ["name"] = new OpenApiString(group.Name),
                    ["tags"] = xTagGroupItem,
                });
            }

            // normilize operations
            foreach (var (_, openApiPathItem) in swaggerDoc.Paths)
            {
                foreach (var (_, operation) in openApiPathItem.Operations)
                {
                    if (operation.Summary == null) continue;
                    var tagName = operation.Tags[0].Name;
                    operation.Summary = Regex.Replace(operation.Summary, @"\[controller\]", swaggerDoc.Tags.FirstOrDefault(x => x.Name == tagName)?.Description ?? tagName);
                }
            }
        }

        private class TagGroupItem
        {
            public string Name { get; set; }

            public int OrderIndex { get; set; }

            public string? Description { get; set; }

            public List<TagItem> Tags { get; set; }
        }

        private class TagItem
        {
            public string Name { get; set; }

            public string? Description { get; set; }
        }
    }
}
