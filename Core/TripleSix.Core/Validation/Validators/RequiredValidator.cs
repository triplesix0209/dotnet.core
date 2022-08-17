using System.ComponentModel.DataAnnotations;
using System.Reflection;
using FluentValidation;
using FluentValidation.Validators;
using TripleSix.Core.Helpers;
using TripleSix.Core.Types;

namespace TripleSix.Core.Validation.Validators
{
    public class RequiredValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        public override string Name => "RequiredValidator";

        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            if (context.InstanceToValidate is not IDto dto) return true;
            if (!dto.IsPropertyChanged(context.PropertyName)) return false;
            var property = dto.GetType().GetProperty(context.PropertyName);
            if (property == null) return true;

            if (property.PropertyType.IsNullableType()
                && property.GetCustomAttributes().Any(x => x.GetType().FullName == "System.Runtime.CompilerServices.NullableAttribute")
                && value == null)
                return false;

            var option = property.GetCustomAttribute<RequiredAttribute>();
            if (value is string str && option != null && !option.AllowEmptyStrings)
                return !str.IsNullOrWhiteSpace();

            return true;
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
            => "Bạn chưa nhập '{PropertyName}'";
    }
}
