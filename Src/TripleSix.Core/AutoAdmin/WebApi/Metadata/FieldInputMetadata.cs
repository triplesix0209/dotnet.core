using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TripleSix.Core.Attributes;
using TripleSix.Core.Dto;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.AutoAdmin
{
    public class FieldInputMetadata : FieldMetadata
    {
        public FieldInputMetadata(Type controllerType, MethodInfo methodType, PropertyInfo fieldType)
            : base(controllerType, methodType, fieldType)
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

            if (Type == "string")
            {
                var stringLength = fieldType.GetCustomAttribute<StringLengthValidateAttribute>();
                Min = stringLength?.MinimumLength;
                Max = stringLength?.MaximumLength;
            }
            else if (fieldType.GetCustomAttribute<RangeValidateAttribute>() is not null)
            {
                var range = fieldType.GetCustomAttribute<RangeValidateAttribute>();
                Min = (double?)range?.Minimum;
                Max = (double?)range?.Maximum;
            }
            else
            {
                Min = fieldType.GetCustomAttribute<MinValidateAttribute>()?.MinValue;
                Max = fieldType.GetCustomAttribute<MaxValidateAttribute>()?.MaxValue;
            }

            IsRequired = fieldType.GetCustomAttribute<RequiredValidateAttribute>() is not null;
            DefaultValue = Operator is null ? fieldType.GetValue(fieldType.ReflectedType.CreateDefaultInstance()) : null;
        }

        public Dictionary<string, string> Operator { get; set; }

        public bool IsRequired { get; set; }

        public double? Min { get; set; }

        public double? Max { get; set; }

        public object DefaultValue { get; set; }
    }
}
