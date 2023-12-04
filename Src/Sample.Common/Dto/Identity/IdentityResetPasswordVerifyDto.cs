using System.ComponentModel;
using TripleSix.CoreOld.Attributes;
using TripleSix.CoreOld.Dto;

namespace Sample.Common.Dto
{
    public class IdentityResetPasswordVerifyDto : DataDto
    {
        [DisplayName("e-mail")]
        [StringLengthValidate(100)]
        [RequiredValidate]
        public string Email { get; set; }
    }
}
