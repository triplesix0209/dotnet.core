using System;
using System.ComponentModel;
using TripleSix.Core.Dto;

namespace Sample.Common.Dto
{
    public class IdentityProfileDto : DataDto
    {
        [DisplayName("định danh tài khoản")]
        public Guid AccountId { get; set; }

        [DisplayName("mã số")]
        public string Code { get; set; }

        [DisplayName("tên gọi")]
        public string Name { get; set; }

        [DisplayName("link ảnh đại diện")]
        public string AvatarLink { get; set; }
    }
}
