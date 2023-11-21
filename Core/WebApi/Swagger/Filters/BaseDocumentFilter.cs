using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.WebApi
{
    public class BaseDocumentFilter : IDocumentFilter
    {
        /// <inheritdoc/>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var tagGroups = new List<TagGroupItem>();
            foreach (var apiDescription in context.ApiDescriptions)
            {
                if (apiDescription.ActionDescriptor is not ControllerActionDescriptor controllerDescriptor) continue;

                var swaggerTagAttr = controllerDescriptor.ControllerTypeInfo.GetCustomAttribute<SwaggerTagAttribute>();
                if (swaggerTagAttr is null && controllerDescriptor.ControllerTypeInfo.IsAssignableToGenericType(typeof(IControllerEndpoint<>)))
                    swaggerTagAttr = controllerDescriptor.ControllerTypeInfo.GetGenericArguments(typeof(IControllerEndpoint<>))[0].GetCustomAttribute<SwaggerTagAttribute>();

                var tag = swaggerDoc.Tags.FirstOrDefault(x => swaggerTagAttr is null
                    ? x.Name == controllerDescriptor.ControllerName
                    : x.Description == swaggerTagAttr.Description);
                if (tag is null)
                {
                    tag = new OpenApiTag
                    {
                        Name = controllerDescriptor.ControllerName,
                        Description = swaggerTagAttr is null ? controllerDescriptor.ControllerName : swaggerTagAttr.Description,
                    };
                    swaggerDoc.Tags.Add(tag);
                }

                var baseController = controllerDescriptor.ControllerTypeInfo.BaseType;
                if (baseController is null) continue;
                var swaggerTagGroupAttr = baseController.GetCustomAttribute<SwaggerTagGroupAttribute>();
                var groupName = swaggerTagGroupAttr?.Description;
                if (groupName is null)
                {
                    groupName = baseController.Name;
                    if (groupName.EndsWith("Controller")) groupName = groupName[..^10];
                }

                var tagGroup = tagGroups.FirstOrDefault(x => x.Name == groupName);
                if (tagGroup == null)
                {
                    tagGroups.Add(new TagGroupItem
                    {
                        Name = groupName,
                        OrderIndex = swaggerTagGroupAttr?.OrderIndex ?? 0,
                        Tags = new HashSet<string> { tag.Name },
                    });
                }
                else
                {
                    tagGroup.Tags.Add(tag.Name);
                }
            }

            foreach (var tag in swaggerDoc.Tags)
            {
                tag.Description = tag.Description?.ToTitleCase() ?? null;
                tag.Extensions.Add("x-displayName", new OpenApiString(tag.Description ?? tag.Name));
            }

            var xTagGroups = new OpenApiArray();
            swaggerDoc.Extensions.Add("x-tagGroups", xTagGroups);
            foreach (var group in tagGroups.OrderBy(x => x.OrderIndex))
            {
                var xTagGroupItem = new OpenApiArray();
                xTagGroupItem.AddRange(group.Tags.Select(x => new OpenApiString(x)));
                xTagGroups.Add(new OpenApiObject()
                {
                    ["name"] = new OpenApiString(group.Name),
                    ["tags"] = xTagGroupItem,
                });
            }

            foreach (var (_, openApiPathItem) in swaggerDoc.Paths)
            {
                foreach (var (_, operation) in openApiPathItem.Operations)
                {
                    if (operation.Summary == null) continue;

                    var tagName = operation.Tags[0].Name;
                    operation.Summary = Regex.Replace(operation.Summary, @"\[controller\]", swaggerDoc.Tags.FirstOrDefault(x => x.Name == tagName)?.Description ?? tagName);
                }
            }

            foreach (var tag in swaggerDoc.Tags)
                tag.Description = tag.Description == tag.Name ? null : tag.Description = tag.Name;
        }

        private class TagGroupItem
        {
            public string Name { get; set; }

            public int OrderIndex { get; set; }

            public HashSet<string> Tags { get; set; }
        }
    }
}
