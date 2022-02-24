using System;
using System.ComponentModel;
using TripleSix.Core.Attributes;
using TripleSix.Core.Dto;

namespace Sample.Common.Dto
{
    public class IdentityResetPasswordDto : DataDto
    {
        [DisplayName("mã phiên reset password")]
        [RequiredValidate]
        public Guid? VerifyId { get; set; }

        [DisplayName("mật khẩu")]
        [RequiredValidate]
        public string Password { get; set; }
    }
}
