using System.ComponentModel.DataAnnotations;
using TripleSix.Core.Dto;
using TripleSix.Core.Extensions;

namespace TripleSix.Core.Attributes
{
    public class RequiredValidateAttribute : ValidationAttribute
    {
        public RequiredValidateAttribute()
        {
            ErrorMessage = "{0} không được bỏ trống";
        }

        protected virtual ValidationResult GenerateErrorResult(ValidationContext context)
        {
            return new ValidationResult(string.Format(ErrorMessage, context?.DisplayName).Trim());
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var model = context?.ObjectInstance as IDataDto;
            if (model is not null && !model.IsPropertyChanged(context.MemberName))
                return ValidationResult.Success;

            if (value == null)
                return GenerateErrorResult(context);
            if (value.GetType().IsArray && (value as object[]).IsNullOrEmpty())
                return GenerateErrorResult(context);

            switch (value)
            {
                case string str when str.IsNullOrWhiteSpace():
                    return GenerateErrorResult(context);
            }

            return ValidationResult.Success;
        }
    }
}