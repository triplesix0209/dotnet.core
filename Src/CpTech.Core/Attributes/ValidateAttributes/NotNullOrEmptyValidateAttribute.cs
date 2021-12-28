using System.ComponentModel.DataAnnotations;
using CpTech.Core.Dto;

namespace CpTech.Core.Attributes
{
    public class NotNullOrEmptyValidateAttribute : ValidationAttribute
    {
        public NotNullOrEmptyValidateAttribute()
        {
            ErrorMessage = "giá trị của {0} không được bỏ trống";
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var model = context.ObjectInstance as IDataDto;
            if (model != null && !model.IsPropertyChanged(context.MemberName))
            {
                return ValidationResult.Success;
            }

            if (value == null)
            {
                return GenerateErrorResult(context);
            }

            if (value.GetType().IsArray && ((object[])value).Length == 0)
            {
                return GenerateErrorResult(context);
            }

            switch (value)
            {
                case string str when string.IsNullOrWhiteSpace(str):
                    return GenerateErrorResult(context);
            }

            return ValidationResult.Success;
        }

        protected virtual ValidationResult GenerateErrorResult(ValidationContext context)
        {
            return new ValidationResult(string.Format(ErrorMessage, context.DisplayName));
        }
    }
}