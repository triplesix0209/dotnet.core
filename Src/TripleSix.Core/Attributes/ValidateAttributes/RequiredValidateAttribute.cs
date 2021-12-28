using System.ComponentModel.DataAnnotations;

namespace TripleSix.Core.Attributes
{
    public class RequiredValidateAttribute : RequiredAttribute
    {
        public RequiredValidateAttribute()
        {
            ErrorMessage = "{0} là thông tin bắt buộc";
        }
    }
}