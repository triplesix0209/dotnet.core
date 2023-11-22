using FluentValidation;
using FluentValidation.Validators;

namespace TripleSix.Core.Validation.Validators
{
    /// <summary>
    /// Must no space validator.
    /// </summary>
    /// <typeparam name="T">Data type.</typeparam>
    /// <typeparam name="TProperty">Property type.</typeparam>
    public class MustNoSpaceValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        /// <inheritdoc/>
        public override string Name => "MustNoSpaceValidator";

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        protected override string GetDefaultMessageTemplate(string errorCode)
            => "'{PropertyName}' không được phép chứa ký tự khoản trắng (space)";
    }
}
