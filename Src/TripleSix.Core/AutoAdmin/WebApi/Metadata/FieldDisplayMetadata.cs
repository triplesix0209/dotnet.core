using System;
using System.Linq;
using System.Reflection;
using TripleSix.Core.Dto;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.AutoAdmin
{
    public class FieldDisplayMetadata : FieldMetadata
    {
        public FieldDisplayMetadata(Type controllerType, MethodInfo methodType, PropertyInfo fieldType)
            : base(controllerType, methodType, fieldType)
        {
            var fieldInfo = fieldType.GetCustomAttribute<AdminFieldAttribute>() ?? new AdminFieldAttribute();

            DisplayBy = fieldInfo.DisplayBy?.Trim().ToCamelCase();
            IsModelKey = fieldInfo.IsModelKey;
            IsModelText = fieldInfo.IsModelText;
        }

        public string DisplayBy { get; set; }

        public bool IsModelKey { get; set; }

        public bool IsModelText { get; set; }

        public static void AfterProcess(params FieldDisplayMetadata[] fields)
        {
            var displayFields = fields.Where(x => fields.Any(y => y.DisplayBy == x.Key));
            foreach (var field in displayFields)
                field.Render = false;

            if (!fields.Any(x => x.IsModelText))
            {
                if (fields.Any(x => x.Key == "name"))
                    fields.First(x => x.Key == "name").IsModelText = true;
                else
                    fields.First(x => x.Key == nameof(IModelDataDto.Code).ToCamelCase()).IsModelText = true;
            }
        }
    }
}
