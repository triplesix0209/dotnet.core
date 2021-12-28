using System.ComponentModel.DataAnnotations;

namespace CpTech.Core.Attributes
{
    public class StringLengthValidateAttribute : StringLengthAttribute
    {
        public StringLengthValidateAttribute(int maximumLength)
            : base(maximumLength)
        {
            ErrorMessage = "{0} không được vượt quá {1} ký tự";
        }

        public StringLengthValidateAttribute(int minimumLength, int maximumLength)
            : base(maximumLength)
        {
            MinimumLength = minimumLength;
            ErrorMessage = "{0} phải có độ dài trong khoảng {2} - {1} ký tự";
        }
    }
}