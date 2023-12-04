using System.Reflection;
using TripleSix.CoreOld.Attributes;
using TripleSix.CoreOld.Helpers;

namespace TripleSix.CoreOld.AutoAdmin
{
    public class FieldInputMetadata : FieldMetadata
    {
        public FieldInputMetadata(ControllerMetadata controllerMetadata, MethodMetadata methodMetadata, PropertyInfo fieldType)
            : base(controllerMetadata, methodMetadata, fieldType)
        {
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
            DefaultValue = fieldType.GetValue(fieldType.ReflectedType.CreateDefaultInstance());
        }

        public bool IsRequired { get; set; }

        public double? Min { get; set; }

        public double? Max { get; set; }

        public object DefaultValue { get; set; }
    }
}
