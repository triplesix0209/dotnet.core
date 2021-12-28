using System.ComponentModel.DataAnnotations;

namespace CpTech.Core.Attributes
{
    public class MinValidateAttribute : ValidationAttribute
    {
        private readonly int _minValue;

        public MinValidateAttribute(int value)
        {
            _minValue = value;
            ErrorMessage = $"giá trị của {{0}} không được nhỏ hơn {_minValue}";
        }

        public override bool IsValid(object value)
        {
            return value == null || (int)value >= _minValue;
        }
    }
}