namespace Sample.Domain.Dto
{
    public class IdentityRefreshDto : BaseDto
    {
        [DisplayName("refresh token")]
        public string? RefreshToken { get; set; }
    }
}
