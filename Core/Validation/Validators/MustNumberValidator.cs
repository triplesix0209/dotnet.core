﻿using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;

namespace TripleSix.Core.Validation.Validators
{
    public class MustNumberValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        public MustNumberValidator()
        {
        }

        /// <inheritdoc/>
        public override string Name => "MustNumberValidator";

        /// <inheritdoc/>
        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            if (value == null)
                return true;

            switch (value)
            {
                case string str when !Regex.IsMatch(str, @"^[0-9]+$"):
                    return false;
            }

            return true;
        }

        /// <inheritdoc/>
        protected override string GetDefaultMessageTemplate(string errorCode)
            => "'{PropertyName}' chỉ được phép chứa chữ số";
    }
}
