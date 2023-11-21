using FluentValidation;
using FluentValidation.Validators;

namespace TripleSix.Core.Validation.Validators
{
    public class MustTrimValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        public MustTrimValidator()
        {
        }

        /// <inheritdoc/>
        public override string Name => "MustTrimValidator";

        /// <inheritdoc/>
        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            if (value == null)
                return true;

            switch (value)
            {
                case string str when str != str.Trim():
                    return false;
            }

            return true;
        }

        /// <inheritdoc/>
        protected override string GetDefaultMessageTemplate(string errorCode)
            => "'{PropertyName}' không được thừa khoảng trắng trước và sau";
    }
}
