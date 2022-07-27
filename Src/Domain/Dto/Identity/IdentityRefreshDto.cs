namespace Sample.Domain.Dto
{
    public class IdentityRefreshDto : DataDto
    {
        [DisplayName("refresh token")]
        public string? RefreshToken { get; set; }
    }
}
