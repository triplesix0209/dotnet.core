﻿using FluentValidation;
using FluentValidation.Validators;

namespace TripleSix.Core.Validation.Validators
{
    public class EnumValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        private List<int> _values;

        public EnumValidator(Type enumType)
        {
            _values = new List<int>();
            foreach (var value in Enum.GetValues(enumType))
                _values.Add((int)value);
        }

        /// <inheritdoc/>
        public override string Name => "EnumValidator";

        /// <inheritdoc/>
        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            if (value == null)
                return true;

            var val = (int)Convert.ChangeType(value, typeof(int));
            return _values.Contains(val);
        }

        /// <inheritdoc/>
        protected override string GetDefaultMessageTemplate(string errorCode)
            => "'{PropertyName}' có giá trị không phù hợp với tập giá trị cho phép";
    }
}
