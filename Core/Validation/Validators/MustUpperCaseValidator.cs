using FluentValidation;
using FluentValidation.Validators;

namespace TripleSix.Core.Validation.Validators
{
    public class MustUpperCaseValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        public MustUpperCaseValidator()
        {
        }

        public override string Name => "MustUpperCaseValidator";

        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            if (value == null)
                return true;

            switch (value)
            {
                case string str when str != str.ToUpper():
                    return false;
            }

            return true;
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
            => "'{PropertyName}' không chỉ được phép sử dụng ký tự hoa";
    }
}
