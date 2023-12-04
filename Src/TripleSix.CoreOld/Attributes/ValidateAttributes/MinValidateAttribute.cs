using System.ComponentModel.DataAnnotations;

namespace TripleSix.CoreOld.Attributes
{
    public class MinValidateAttribute : ValidationAttribute
    {
        public MinValidateAttribute(int value)
        {
            MinValue = value;
            ErrorMessage = $"giá trị của {{0}} không được nhỏ hơn {MinValue}";
        }

        public int MinValue { get; }

        public override bool IsValid(object value)
        {
            return value == null || (int)value >= MinValue;
        }
    }
}