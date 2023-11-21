using FluentValidation;
using FluentValidation.Validators;

namespace TripleSix.Core.Validation.Validators
{
    public class MustNoSpaceValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        public MustNoSpaceValidator()
        {
        }

        public override string Name => "MustNoSpaceValidator";

        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            if (value == null)
                return true;

            switch (value)
            {
                case string str when str.Contains(' '):
                    return false;
            }

            return true;
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
            => "'{PropertyName}' không được phép chứa ký tự khoản trắng (space)";
    }
}
