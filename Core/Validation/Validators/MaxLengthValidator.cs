using System.Collections;
using FluentValidation;
using FluentValidation.Validators;

namespace TripleSix.Core.Validation.Validators
{
    /// <summary>
    /// Max length validator.
    /// </summary>
    /// <typeparam name="T">Data type.</typeparam>
    /// <typeparam name="TProperty">Property type.</typeparam>
    public class MaxLengthValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        private int _length;

        /// <summary>
        /// Max length validator.
        /// </summary>
        /// <param name="length">Max lenght allowed.</param>
        public MaxLengthValidator(int length)
        {
            _length = length;
        }

        /// <inheritdoc/>
        public override string Name => "MaxLengthValidator";

        /// <inheritdoc/>
        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            context.MessageFormatter.AppendArgument("Length", _length);

            if (value == null)
                return true;

            switch (value)
            {
                case string str when str.Length > _length:
                case ICollection collection when collection.Count > _length:
                case Array array when array.Length > _length:
                    return false;
            }

            return true;
        }

        /// <inheritdoc/>
        protected override string GetDefaultMessageTemplate(string errorCode)
            => "'{PropertyName}' phải có độ dài <= {Length}";
    }
}
