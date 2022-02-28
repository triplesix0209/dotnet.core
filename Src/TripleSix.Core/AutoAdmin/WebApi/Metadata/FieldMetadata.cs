using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Newtonsoft.Json;
using TripleSix.Core.Dto;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.AutoAdmin
{
    public class FieldMetadata
    {
        public FieldMetadata(ControllerMetadata controllerMetadata, MethodMetadata methodMetadata, PropertyInfo fieldType)
        {
            var fieldInfo = fieldType.GetCustomAttribute<AdminFieldAttribute>() ?? new AdminFieldAttribute();

            FieldType = fieldType;
            Render = fieldInfo.Render;
            Key = fieldType.Name.ToCamelCase();
            GridCol = fieldInfo.GridCol;
            ScriptDisplay = fieldInfo.ScriptDisplay?.Trim();

            if (fieldInfo.GroupCode.IsNotNullOrWhiteSpace() || fieldInfo.GroupName.IsNotNullOrWhiteSpace())
            {
                GroupData = new GroupMetadata
                {
                    Code = fieldInfo.GroupCode.IsNotNullOrWhiteSpace()
                        ? fieldInfo.GroupCode.Trim().ToKebabCase()
                        : fieldInfo.GroupName.Trim().ToKebabCase(),
                    Name = fieldInfo.GroupName.IsNotNullOrWhiteSpace()
                        ? fieldInfo.GroupName.Trim()
                        : fieldInfo.GroupCode.Trim(),
                    Icon = fieldInfo.GroupIcon?.Trim(),
                };

                Group = GroupData.Code;
            }

            Name = fieldType.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName.Trim();
            if (Name.IsNullOrWhiteSpace())
                Name = Key;
            else if (Name.ToLower().StartsWith("lọc theo"))
                Name = Name["lọc theo".Length..].Trim();
            Description = fieldType.GetCustomAttribute<DescriptionAttribute>()?.Description.Trim();

            LoadFieldType(controllerMetadata, methodMetadata, fieldType);
        }

        public bool Render { get; set; }

        public string Key { get; set; }

        public string Type { get; set; }

        public string Group { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Dictionary<string, string> Enum { get; set; }

        public int GridCol { get; set; }

        public string ScriptDisplay { get; set; }

        public ModelMetadata ModelController { get; set; }

        [JsonIgnore]
        public PropertyInfo FieldType { get; set; }

        [JsonIgnore]
        public GroupMetadata GroupData { get; set; }

        protected virtual void LoadFieldType(ControllerMetadata controllerMetadata, MethodMetadata methodMetadata, PropertyInfo fieldType)
        {
            var controllerType = controllerMetadata.ControllerType;
            var methodType = methodMetadata.MethodType;
            var propertyType = fieldType.PropertyType.GetUnderlyingType();
            if (propertyType.IsSubclassOfRawGeneric(typeof(FilterParameter<>))
                || propertyType.IsSubclassOfRawGeneric(typeof(FilterParameterNumber<>)))
            {
                propertyType = propertyType.GetGenericArguments()[0];
            }
            else if (propertyType.IsAssignableTo<FilterParameterDatetime>())
            {
                propertyType = typeof(DateTime);
            }
            else if (propertyType.IsAssignableTo<FilterParameterString>())
            {
                propertyType = typeof(string);
            }

            var fieldInfo = fieldType.GetCustomAttribute<AdminFieldAttribute>() ?? new AdminFieldAttribute();
            Type = propertyType.Name.ToCamelCase();

            if (propertyType.IsEnum)
            {
                Type = "enum";
                Enum = new Dictionary<string, string>();

                var values = EnumHelper.GetValues(propertyType);
                foreach (var value in values)
                    Enum.Add(((int)value).ToString(), EnumHelper.GetDescription(propertyType, value));
            }
            else if (propertyType == typeof(bool))
            {
                Type = "enum";
                Enum = new Dictionary<string, string>()
                {
                    { "true", "có" },
                    { "false", "không" },
                };
            }
            else if (fieldInfo.Type == AdminFieldTypes.Media)
            {
                Type = nameof(AdminFieldTypes.Media).ToCamelCase();
            }
            else if (fieldInfo.Type == AdminFieldTypes.HTML)
            {
                Type = nameof(AdminFieldTypes.HTML).ToLower();
            }
            else if (fieldInfo.Type == AdminFieldTypes.HierarchyParentId)
            {
                var modelType = fieldInfo.ModelType;
                if (modelType is null && fieldInfo.Type == AdminFieldTypes.HierarchyParentId)
                {
                    modelType = methodType.GetCustomAttribute<AdminMethodAttribute>().AdminType
                        ?? controllerType.GetCustomAttribute<AdminControllerAttribute>().AdminType;
                }

                Type = "ParentId".ToCamelCase();
                ModelController = new ModelMetadata(controllerMetadata, methodMetadata, this, modelType);
            }
            else if (propertyType == typeof(Guid))
            {
                Type = "id";
                ModelController = new ModelMetadata(controllerMetadata, methodMetadata, this, fieldInfo.ModelType);
            }
        }
    }
}
