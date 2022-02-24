using System.ComponentModel;
using TripleSix.Core.Attributes;
using TripleSix.Core.Dto;

namespace Sample.Common.Dto
{
    public class IdentityRegisterDto : DataDto
    {
        [DisplayName("e-mail")]
        [StringLengthValidate(100)]
        [RequiredValidate]
        public string Email { get; set; }

        [DisplayName("tên gọi")]
        [StringLengthValidate(100)]
        [RequiredValidate]
        public string Name { get; set; }

        [DisplayName("mật khẩu")]
        [RequiredValidate]
        public string Password { get; set; }
    }
}
