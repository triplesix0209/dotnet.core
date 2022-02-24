using System.ComponentModel;
using TripleSix.Core.Attributes;
using TripleSix.Core.Dto;

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
