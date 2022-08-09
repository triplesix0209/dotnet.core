namespace Sample.Domain.Dto
{
    public class IdentityRegisterInputDto : BaseDto
    {
        [DisplayName("tên gọi")]
        [Required]
        public string Name { get; set; }

        [DisplayName("tên đăng nhập")]
        [Required]
        public string Username { get; set; }

        [DisplayName("mật khẩu")]
        [Required]
        public string Password { get; set; }
    }
}
