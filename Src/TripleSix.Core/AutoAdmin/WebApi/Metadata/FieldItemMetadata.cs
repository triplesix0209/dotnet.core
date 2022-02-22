using System;
using System.Reflection;

namespace TripleSix.Core.AutoAdmin
{
    public class FieldItemMetadata : FieldDisplayMetadata
    {
        public FieldItemMetadata(Type controllerType, MethodInfo methodType, PropertyInfo fieldType)
            : base(controllerType, methodType, fieldType)
        {
            var fieldInfo = fieldType.GetCustomAttribute<AdminFieldAttribute>() ?? new AdminFieldAttribute();

            Sortable = fieldInfo.Sortable;
        }

        public bool Sortable { get; set; }
    }
}
