using System.ComponentModel;
using TripleSix.Core.Dto;

namespace Sample.Common.Dto
{
    public class IdentityTokenDto : DataDto
    {
        [DisplayName("token truy cập")]
        public string AccessToken { get; set; }

        [DisplayName("thời gian sống của access token (giây)")]
        public int AccessTokenLifetime { get; set; }

        [DisplayName("token dùng để refresh")]
        public string RefreshToken { get; set; }

        [DisplayName("thời gian sống của refresh token (giây)")]
        public int RefreshTokenLifetime { get; set; }

        [DisplayName("thông tin cá nhân")]
        public IdentityProfileDto Profile { get; set; }
    }
}
