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

            return false;
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
            => "Bạn chưa nhập '{PropertyName}'";
    }
}
