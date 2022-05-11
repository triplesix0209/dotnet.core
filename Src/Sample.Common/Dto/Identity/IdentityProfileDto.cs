using System;
using System.ComponentModel;
using TripleSix.Core.Dto;

namespace Sample.Common.Dto
{
    public class IdentityProfileDto : DataDto
    {
        [DisplayName("định danh tài khoản")]
        public Guid AccountId { get; set; }

        [DisplayName("tên gọi")]
        public string Name { get; set; }

        [DisplayName("e-mail")]
        public string Email { get; set; }

        [DisplayName("tên đăng nhập")]
        public string Username { get; set; }

        [DisplayName("link ảnh đại diện")]
        public string AvatarLink { get; set; }
    }
}
