using System.ComponentModel;
using TripleSix.Core.Attributes;
using TripleSix.Core.Dto;

namespace Sample.Common.Dto
{
    public class IdentitySetPasswordDto : DataDto
    {
        [DisplayName("tên đăng nhập")]
        [StringLengthValidate(100)]
        [RequiredValidate]
        public string Username { get; set; }

        [DisplayName("mật khẩu")]
        [StringLengthValidate(100)]
        [RequiredValidate]
        public string Password { get; set; }
    }
}
