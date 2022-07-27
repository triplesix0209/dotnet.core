namespace Sample.Domain.Services
{
    public interface IIdentityService : IService
    {
        Task<IdentityTokenDto> LoginByUsernamePassword(string username, string password);

        Task<IdentityProfileDto> GetProfileByAccountId(Guid accountId);

        Task<IdentityTokenDto> RefreshToken(string sessionCode);

        Task ClearSession(Guid accountId);

        Task ClearAllExpiredSession();
    }
}
