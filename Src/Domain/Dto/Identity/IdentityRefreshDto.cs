namespace Sample.Domain.Dto
{
    public class IdentityRefreshDto : BaseDto
    {
        [DisplayName("refresh token")]
        [Required]
        public string RefreshToken { get; set; }
    }
}
