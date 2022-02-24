using System;
using System.Threading.Tasks;
using Sample.Common.Dto;
using TripleSix.Core.Dto;
using TripleSix.Core.Services;

namespace Sample.Middle.Services
{
    public interface IIdentityService : IService
    {
        Task<string> CreateSession(IIdentity identity, Guid accountId);

        Task ExtendSession(IIdentity identity, Guid sessionId);

        Task ClearSession(IIdentity identity, Guid accountId);

        Task ClearExpiredSession(IIdentity identity);

        Task<IdentityTokenDto> GenerateTokenByAccountId(IIdentity identity, Guid accountId, string sessionToken = null);

        Task<IdentityTokenDto> GenerateTokenBySessionToken(IIdentity identity, string sessionToken, bool extendSession = false);

        Task<IdentityProfileDto> GetProfileByAccountId(IIdentity identity, Guid accountId);

        Task<IdentityTokenDto> LoginByUsernamePassword(IIdentity identity, IdentityLoginUsernamePasswordDto input);
    }
}
