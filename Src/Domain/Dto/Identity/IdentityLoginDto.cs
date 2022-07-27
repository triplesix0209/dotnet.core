namespace Sample.Domain.Dto
{
    public class IdentityLoginDto : DataDto
    {
        [DisplayName("tên đăng nhập")]
        public string? Username { get; set; }

        [DisplayName("mật khẩu")]
        public string? Password { get; set; }
    }
}
