using System.ComponentModel.DataAnnotations;

namespace CpTech.Core.Attributes
{
    public class RegexValidateAttribute : RegularExpressionAttribute
    {
        public RegexValidateAttribute(string pattern)
            : base(pattern)
        {
            ErrorMessage = "giá trị của {0} phải có định dạng {1}";
        }

        public RegexValidateAttribute(string pattern, string patternName)
            : base(pattern)
        {
            ErrorMessage = $"{{0}} phải là {patternName}";
        }
    }
}