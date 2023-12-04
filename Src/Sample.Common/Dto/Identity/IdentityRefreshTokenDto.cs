using System.ComponentModel;

namespace Sample.Common.Dto
{
    public class IdentityRefreshTokenDto : DataDto
    {
        [DisplayName("refresh token")]
        [StringLengthValidate(100)]
        [RequiredValidate]
        public string RefreshToken { get; set; }
    }
}
