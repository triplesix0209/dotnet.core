using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;

namespace TripleSix.Core.Validation.Validators
{
    /// <summary>
    /// Must valid regular expression validator.
    /// </summary>
    /// <typeparam name="T">Data type.</typeparam>
    /// <typeparam name="TProperty">Property type.</typeparam>
    public class MustRegExrValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        private string _patternExr;
        private string? _patternName;

        /// <summary>
        /// Min value validator.
        /// </summary>
        /// <param name="patternExr">regular expression allowed.</param>
        /// <param name="patternName">pattern's name.</param>
        public MustRegExrValidator(string patternExr, string? patternName = null)
        {
            _patternExr = patternExr;
            _patternName = patternName;
        }

        /// <inheritdoc/>
        public override string Name => "MustRegExrValidator";

        /// <inheritdoc/>
        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            if (value == null)
                return true;

            switch (value)
            {
                case string str when !Regex.IsMatch(str, _patternExr):
                    return false;
            }

            return true;
        }

        /// <inheritdoc/>
        protected override string GetDefaultMessageTemplate(string errorCode)
            => "'{PropertyName}' không hợp lệ với " + (_patternName != null ? _patternExr : _patternName);
    }
}
