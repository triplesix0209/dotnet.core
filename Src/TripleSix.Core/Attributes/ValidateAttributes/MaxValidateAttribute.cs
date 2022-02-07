using System.ComponentModel.DataAnnotations;

namespace TripleSix.Core.Attributes
{
    public class MaxValidateAttribute : ValidationAttribute
    {
        public MaxValidateAttribute(int value)
        {
            MaxValue = value;
            ErrorMessage = $"giá trị của {{0}} không được lớn hơn {MaxValue}";
        }

        public int MaxValue { get; }

        public override bool IsValid(object value)
        {
            return value == null || (int)value <= MaxValue;
        }
    }
}