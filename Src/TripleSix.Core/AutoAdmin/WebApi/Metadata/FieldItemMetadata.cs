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
                var sortColumn = fieldInfo.SortByColumn?.Trim();
                if (sortColumn.IsNullOrWhiteSpace())
                {
                    var entityType = methodType.GetCustomAttribute<AdminMethodAttribute>().EntityType
                        ?? controllerType.GetCustomAttribute<AdminControllerAttribute>().EntityType;
                    if (entityType.GetProperty(fieldType.Name) is not null)
                        sortColumn = fieldType.Name.ToCamelCase();
                }

                if (sortColumn.IsNotNullOrWhiteSpace())
                    SortColumn = sortColumn;
            }
        }

        public string SortColumn { get; set; }
    }
}
