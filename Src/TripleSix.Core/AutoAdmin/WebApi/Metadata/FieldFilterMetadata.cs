using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TripleSix.Core.Dto;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.AutoAdmin
{
    public class FieldFilterMetadata : FieldInputMetadata
    {
        public FieldFilterMetadata(ControllerMetadata controllerMetadata, MethodMetadata methodMetadata, PropertyInfo fieldType)
            : base(controllerMetadata, methodMetadata, fieldType)
        {
            var fieldInfo = fieldType.GetCustomAttribute<AdminFieldAttribute>() ?? new AdminFieldAttribute();
            var propertyType = fieldType.PropertyType;

            if (propertyType.IsSubclassOfRawGeneric(typeof(FilterParameter<>)))
            {
                Operator = EnumHelper.GetValues<FilterParameterOperators>()
                    .ToDictionary(x => x.ToString(), x => EnumHelper.GetDescription(x));
            }
            else if (propertyType.IsAssignableTo<FilterParameterDatetime>())
            {
                Operator = EnumHelper.GetValues<FilterParameterDatetimeOperators>()
                    .ToDictionary(x => x.ToString(), x => EnumHelper.GetDescription(x));
            }
            else if (propertyType.IsSubclassOfRawGeneric(typeof(FilterParameterNumber<>)))
            {
                Operator = EnumHelper.GetValues<FilterParameterNumberOperators>()
                    .ToDictionary(x => x.ToString(), x => EnumHelper.GetDescription(x));
            }
            else if (propertyType.IsAssignableTo<FilterParameterString>())
            {
                Operator = EnumHelper.GetValues<FilterParameterStringOperators>()
                    .ToDictionary(x => x.ToString(), x => EnumHelper.GetDescription(x));
            }

            //DefaultValue = Operator is null ? fieldType.GetValue(fieldType.ReflectedType.CreateDefaultInstance()) : null;
        }

        public Dictionary<string, string> Operator { get; set; }
    }
}
