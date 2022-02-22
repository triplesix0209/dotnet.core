using System;
using System.Linq;
using System.Reflection;
using Swashbuckle.AspNetCore.Annotations;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.AutoAdmin
{
    public class ControllerMetadata
    {
        public ControllerMetadata(Type controllerType)
        {
            var controllerInfo = controllerType.GetCustomAttribute<AdminControllerAttribute>();
            var controllerName = controllerType.Name.Substring(0, controllerType.Name.LastIndexOf("Controller"));

            Code = controllerName.ToKebabCase();
            Icon = controllerInfo.Icon?.Trim();
            PermissionGroup = controllerInfo.PermissionGroup.IsNullOrWhiteSpace()
                ? controllerName.ToCamelCase()
                : controllerInfo.PermissionGroup;

            Name = controllerInfo is not null && controllerInfo.Name.IsNotNullOrWhiteSpace()
                ? controllerInfo.Name.Trim()
                : "[controller]";
            if (Name.Contains("[controller]"))
                Name = Name.Replace("[controller]", controllerType.GetCustomAttribute<SwaggerTagAttribute>()?.Description ?? controllerName);

            if (controllerInfo.GroupName.IsNotNullOrWhiteSpace())
            {
                Group = new GroupMetadata
                {
                    Code = controllerInfo.GroupCode.IsNotNullOrWhiteSpace()
                        ? controllerInfo.GroupCode.Trim()
                        : controllerInfo.GroupName.RemoveVietnameseSign().Trim().ToKebabCase(),
                    Name = controllerInfo.GroupName.Trim(),
                    Icon = controllerInfo.GroupIcon?.Trim(),
                };
            }

            Methods = MethodMetadata.GetListMethodOfController(controllerType)
                .Select(methodType => MethodMetadata.GenerateMethodMetadata(controllerType, methodType))
                .ToArray();

            Render = controllerInfo.RenderOnMenu && Methods.Any(x => x.Type == AdminMethodTypes.List);
        }

        public bool Render { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; }

        public GroupMetadata Group { get; set; }

        public string PermissionGroup { get; set; }

        public MethodMetadata[] Methods { get; set; }
    }
}
