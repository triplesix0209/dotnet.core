using FluentValidation;
using FluentValidation.Validators;

namespace TripleSix.Core.Validation.Validators
{
    public class MustTrimValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        public MustTrimValidator()
        {
        }

        public override string Name => "MustTrimValidator";

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

        protected override string GetDefaultMessageTemplate(string errorCode)
            => "'{PropertyName}' không được thừa khoảng trắng trước và sau";
    }
}
