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

        /// <inheritdoc/>
        public override string Name => "MustWordNumberValidator";

        /// <inheritdoc/>
        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            if (value == null)
                return true;

            switch (value)
            {
                case string str when !Regex.IsMatch(str, @"^[a-zA-Z0-9]+$"):
                    return false;
            }

            return true;
        }

        /// <inheritdoc/>
        protected override string GetDefaultMessageTemplate(string errorCode)
            => "'{PropertyName}' chỉ được phép chứa chữ cái hoặc chữ số";
    }
}
