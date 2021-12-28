using System.ComponentModel.DataAnnotations;

namespace CpTech.Core.Attributes
{
    public class RequiredValidateAttribute : RequiredAttribute
    {
        public RequiredValidateAttribute()
        {
            ErrorMessage = "{0} là thông tin bắt buộc";
        }
    }
}