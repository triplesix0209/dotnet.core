using System.ComponentModel;

namespace Sample.Common.Dto
{
    public class IdentityLoginUsernamePasswordDto : DataDto
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
