using FluentValidation;
using FluentValidation.Validators;

namespace TripleSix.Core.Validation.Validators
{
    /// <summary>
    /// Min value validator.
    /// </summary>
    /// <typeparam name="T">Data type.</typeparam>
    /// <typeparam name="TProperty">Property type.</typeparam>
    public class MinValueValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        private long _value;

        /// <summary>
        /// Min value validator.
        /// </summary>
        /// <param name="value">min valued allowed.</param>
        public MinValueValidator(long value)
        {
            _value = value;
        }

        /// <inheritdoc/>
        public override string Name => "MinValueValidator";

        /// <inheritdoc/>
        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            context.MessageFormatter.AppendArgument("Value", _value);

            if (value == null)
                return true;

            if (_value.CompareTo(Convert.ChangeType(value, TypeCode.Int64)) > 0)
                return false;

            return true;
        }

        /// <inheritdoc/>
        protected override string GetDefaultMessageTemplate(string errorCode)
            => "'{PropertyName}' phải >= {Value}";
    }
}
