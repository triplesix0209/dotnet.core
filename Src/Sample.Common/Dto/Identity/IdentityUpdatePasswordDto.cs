using System.ComponentModel;
using TripleSix.Core.Attributes;
using TripleSix.Core.Dto;

namespace Sample.Common.Dto
{
    public class IdentityUpdatePasswordDto : DataDto
    {
        [DisplayName("mật khẩu cũ")]
        [StringLengthValidate(100)]
        [RequiredValidate]
        public string OldPassword { get; set; }

        [DisplayName("mật khẩu mới")]
        [StringLengthValidate(100)]
        [RequiredValidate]
        public string NewPassword { get; set; }
    }
}
