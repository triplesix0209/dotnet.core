﻿using FluentValidation;
using FluentValidation.Validators;

namespace TripleSix.Core.Validation.Validators
{
    public class MustLowerCaseValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        public MustLowerCaseValidator()
        {
        }

        /// <inheritdoc/>
        public override string Name => "MustLowerCaseValidator";

        /// <inheritdoc/>
        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            if (value == null)
                return true;

            switch (value)
            {
                case string str when str != str.ToLower():
                    return false;
            }

            return true;
        }

        /// <inheritdoc/>
        protected override string GetDefaultMessageTemplate(string errorCode)
            => "'{PropertyName}' chỉ được phép sử dụng ký tự thường";
    }
}
