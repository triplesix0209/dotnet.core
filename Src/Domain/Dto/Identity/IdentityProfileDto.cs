namespace Sample.Domain.Dto
{
    public class IdentityProfileDto : BaseDto
    {
        [DisplayName("định danh tài khoản")]
        public Guid AccountId { get; set; }

        [DisplayName("tên gọi")]
        public string Name { get; set; }

        [DisplayName("tên đăng nhập")]
        public string? Username { get; set; }

        [DisplayName("link ảnh đại diện")]
        public string? AvatarLink { get; set; }
    }
}
