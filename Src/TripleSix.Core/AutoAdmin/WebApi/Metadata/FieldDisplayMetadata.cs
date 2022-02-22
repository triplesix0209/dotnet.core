using System;
using System.Reflection;

namespace TripleSix.Core.AutoAdmin
{
    public class FieldDisplayMetadata : FieldMetadata
    {
        public FieldDisplayMetadata(Type controllerType, MethodInfo methodType, PropertyInfo fieldType)
            : base(controllerType, methodType, fieldType)
        {
            var fieldInfo = fieldType.GetCustomAttribute<AdminFieldAttribute>() ?? new AdminFieldAttribute();

            IsModelKey = fieldInfo.IsModelKey;
            IsModelText = fieldInfo.IsModelText;
        }

        public bool IsModelKey { get; set; }

        public bool IsModelText { get; set; }
    }
}
