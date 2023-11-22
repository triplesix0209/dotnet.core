using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;

namespace TripleSix.Core.Validation.Validators
{
    /// <summary>
    /// Must phone validator.
    /// </summary>
    /// <typeparam name="T">Data type.</typeparam>
    /// <typeparam name="TProperty">Property type.</typeparam>
    public class MustPhoneValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        private int _minLength;
        private int? _maxLength;

        /// <summary>
        /// Must phone validator.
        /// </summary>
        /// <param name="minLength">Min length allowed.</param>
        /// <param name="maxLength">Max length allowed.</param>
        public MustPhoneValidator(int minLength = 10, int? maxLength = null)
        {
            _minLength = minLength;
            _maxLength = maxLength;
        }

        /// <inheritdoc/>
        public override string Name => "MustPhoneValidator";

        /// <inheritdoc/>
        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            context.MessageFormatter.AppendArgument("MinLength", _minLength);
            context.MessageFormatter.AppendArgument("MaxLength", _maxLength);

            if (value == null)
                return true;

            var pattern = "^[0-9]{" + _minLength + "," + (_maxLength.HasValue ? _maxLength : string.Empty) + "}$";
            switch (value)
            {
                case string str when !Regex.IsMatch(str, pattern):
                    return false;
            }

            return true;
        }

        /// <inheritdoc/>
        protected override string GetDefaultMessageTemplate(string errorCode)
            => "'{PropertyName}' phải là số điện thoại";
    }
}
