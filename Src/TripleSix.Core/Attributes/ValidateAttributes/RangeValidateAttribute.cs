using System.ComponentModel.DataAnnotations;

namespace TripleSix.Core.Attributes
{
    public class RangeValidateAttribute : RangeAttribute
    {
        public RangeValidateAttribute(double minimum, double maximum)
            : base(minimum, maximum)
        {
            ErrorMessage = "giá trị của {0} phải nằm trong khoảng {1} - {2}";
        }
    }
}