namespace Sample.Domain.Dto
{
    public class IdentityLoginDto : BaseDto
    {
        [DisplayName("tên đăng nhập")]
        [Required]
        public string Username { get; set; }

        [DisplayName("mật khẩu")]
        [Required]
        public string Password { get; set; }
    }
}
