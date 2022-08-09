namespace Sample.Domain.Dto
{
    public class IdentityRegisterResultDto : BaseDto
    {
        [DisplayName("tài khoản đã hoạt động")]
        public bool IsAccountActivated { get; set; }

        [DisplayName("thông báo")]
        public string Message { get; set; }
    }
}
