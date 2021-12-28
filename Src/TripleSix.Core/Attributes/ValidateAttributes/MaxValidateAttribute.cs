using System.ComponentModel.DataAnnotations;

namespace TripleSix.Core.Attributes
{
    public class MaxValidateAttribute : ValidationAttribute
    {
        private readonly int _maxValue;

        public MaxValidateAttribute(int value)
        {
            _maxValue = value;
            ErrorMessage = $"giá trị của {{0}} không được lớn hơn {_maxValue}";
        }

        public override bool IsValid(object value)
        {
            return value == null || (int)value <= _maxValue;
        }
    }
}