using FluentValidation;
using FluentValidation.Validators;

namespace TripleSix.Core.Validation.Validators
{
    public class MinValueValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        private long _value;

        public MinValueValidator(long value)
        {
            _value = value;
        }

        public override string Name => "MinValueValidator";

        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            context.MessageFormatter.AppendArgument("Value", _value);

            if (value == null)
                return true;

            if (_value.CompareTo(Convert.ChangeType(value, TypeCode.Int64)) > 0)
                return false;

            return true;
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
            => "'{PropertyName}' phải >= {Value}";
    }
}
