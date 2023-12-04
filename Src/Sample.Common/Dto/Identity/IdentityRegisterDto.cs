using System.ComponentModel;
using TripleSix.CoreOld.Attributes;
using TripleSix.CoreOld.Dto;

namespace Sample.Common.Dto
{
    public class IdentityRegisterDto : DataDto
    {
        [DisplayName("tên gọi")]
        [StringLengthValidate(100)]
        [RequiredValidate]
        public string Name { get; set; }

        [DisplayName("e-mail")]
        [StringLengthValidate(100)]
        [RequiredValidate]
        public string Email { get; set; }

        [DisplayName("tên đăng nhập")]
        [StringLengthValidate(100)]
        [RequiredValidate]
        public string Username { get; set; }

        [DisplayName("mật khẩu")]
        [RequiredValidate]
        public string Password { get; set; }
    }
}
