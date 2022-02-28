using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.AutoAdmin
{
    public class ControllerMetadata
    {
        public ControllerMetadata(Type controllerType)
        {
            var controllerInfo = controllerType.GetCustomAttribute<AdminControllerAttribute>();

            ControllerType = controllerType;
            ControllerCode = controllerType.Name.Substring(0, controllerType.Name.LastIndexOf("Controller"));
            Code = ControllerCode.ToKebabCase();
            Icon = controllerInfo.Icon?.Trim();
            PermissionGroup = controllerInfo.PermissionGroup.IsNullOrWhiteSpace()
                ? ControllerCode.ToCamelCase()
                : controllerInfo.PermissionGroup;

            Name = controllerInfo is not null && controllerInfo.Name.IsNotNullOrWhiteSpace()
                ? controllerInfo.Name.Trim()
                : "[controller]";
            if (Name.Contains("[controller]"))
                Name = Name.Replace("[controller]", controllerType.GetCustomAttribute<SwaggerTagAttribute>()?.Description ?? ControllerCode);

            if (controllerInfo.GroupName.IsNotNullOrWhiteSpace())
            {
                GroupData = new GroupMetadata
                {
                    Code = controllerInfo.GroupCode.IsNotNullOrWhiteSpace()
                        ? controllerInfo.GroupCode.Trim()
                        : controllerInfo.GroupName.RemoveVietnameseSign().Trim().ToKebabCase(),
                    Name = controllerInfo.GroupName.Trim(),
                    Icon = controllerInfo.GroupIcon?.Trim(),
                };

                Group = GroupData.Code;
            }

            MethodData = MethodMetadata.GetListMethodOfController(controllerType)
                .Select(methodType => MethodMetadata.GenerateMethodMetadata(this, methodType))
                .ToArray();

            Render = controllerInfo.RenderOnMenu && MethodData.Any(x => x.Type == AdminMethodTypes.List);
        }

        public bool Render { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; }

        public string Group { get; set; }

        public string PermissionGroup { get; set; }

        [JsonIgnore]
        public Type ControllerType { get; set; }

        [JsonIgnore]
        public string ControllerCode { get; set; }

        [JsonIgnore]
        public GroupMetadata GroupData { get; set; }

        [JsonIgnore]
        public MethodMetadata[] MethodData { get; set; }
    }
}
