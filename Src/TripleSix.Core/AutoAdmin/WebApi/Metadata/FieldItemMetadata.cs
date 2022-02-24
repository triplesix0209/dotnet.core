using System;
using System.Reflection;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.AutoAdmin
{
    public class FieldItemMetadata : FieldDisplayMetadata
    {
        public FieldItemMetadata(Type controllerType, MethodInfo methodType, PropertyInfo fieldType)
            : base(controllerType, methodType, fieldType)
        {
            var fieldInfo = fieldType.GetCustomAttribute<AdminFieldAttribute>() ?? new AdminFieldAttribute();

            if (fieldInfo.Sortable)
            {
                var sortColumn = fieldInfo.SortByColumn;
                if (sortColumn.IsNullOrWhiteSpace())
                    sortColumn = fieldType.Name.ToCamelCase();
                SortColumn = sortColumn;
            }
        }

        public string SortColumn { get; set; }
    }
}
