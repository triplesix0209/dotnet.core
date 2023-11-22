using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;

namespace TripleSix.Core.Validation.Validators
{
    /// <summary>
    /// Must email validator.
    /// </summary>
    /// <typeparam name="T">Data type.</typeparam>
    /// <typeparam name="TProperty">Property type.</typeparam>
    public class MustEmailValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        /// <inheritdoc/>
        public override string Name => "MustEmailValidator";

        /// <inheritdoc/>
        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            if (value == null)
                return true;

            switch (value)
            {
                case string str when !Regex.IsMatch(str, @"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$"):
                    return false;
            }

            return true;
        }

        /// <inheritdoc/>
        protected override string GetDefaultMessageTemplate(string errorCode)
            => "'{PropertyName}' phải là e-mail";
    }
}
