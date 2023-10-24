using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;

namespace TripleSix.Core.Validation.Validators
{
    public class MustWordNumberValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        public MustWordNumberValidator()
        {
        }

        public override string Name => "MustWordNumberValidator";

        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            if (value == null)
                return true;

            switch (value)
            {
                case string str when !Regex.IsMatch(str, @"^[a-zA-Z1-9]+$"):
                    return false;
            }

            return true;
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
            => "'{PropertyName}' chỉ được phép chứa chữ cái hoặc chữ số";
    }
}
