using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Sample.Common;
using Sample.Common.Dto;
using Sample.Common.Enum;
using Sample.Data.Entities;
using Sample.Data.Repositories;

namespace Sample.Middle.Services
{
    public class IdentityService : BaseService,
        IIdentityService
    {
        public AccountSessionRepository AccountSessionRepo { get; set; }

        public IAccountService AccountService { get; set; }

        public async Task<string> CreateSession(IIdentity identity, Guid accountId)
        {
            var tokenLifetime = Configuration.GetValue<int>("Identity:SessionTokenLifetime");
            var session = await AccountSessionRepo.Query
                .Where(x => x.AccountId == accountId)
                .FirstOrDefaultAsync<AccountSessionEntity>(Mapper);

            if (session == null)
            {
                session = new AccountSessionEntity
                {
                    AccountId = accountId,
                    Code = Guid.NewGuid().ToString().Replace(@"-", string.Empty),
                    ExpiryDatetime = DateTime.UtcNow.AddSeconds(tokenLifetime),
                };

                AccountSessionRepo.Create(session);
                await AccountSessionRepo.SaveChanges();
            }
            else
            {
                await ExtendSession(identity, session.Id);
            }

            return session.Code;
        }

        public async Task ExtendSession(IIdentity identity, Guid sessionId)
        {
            var tokenLifetime = Configuration.GetValue<int>("Identity:SessionTokenLifetime");
            var session = await AccountSessionRepo.Query
                .Where(x => x.Id == sessionId)
                .FirstAsync<AccountSessionEntity>(Mapper);

            session.ExpiryDatetime = DateTime.UtcNow.AddSeconds(tokenLifetime);
            AccountSessionRepo.Update(session);
            await AccountSessionRepo.SaveChanges();
        }

        public async Task ClearSession(IIdentity identity, Guid accountId)
        {
            var sessions = await AccountSessionRepo.Query
                .Where(x => x.AccountId == accountId)
                .ToArrayAsync<AccountSessionEntity>(Mapper);

            if (sessions.IsNullOrEmpty()) return;

            AccountSessionRepo.Delete(sessions);
            await AccountSessionRepo.SaveChanges();
        }

        public async Task ClearExpiredSession(IIdentity identity)
        {
            var sessions = await AccountSessionRepo.Query
                .Where(x => DateTime.UtcNow > x.ExpiryDatetime)
                .ToArrayAsync<AccountSessionEntity>(Mapper);

            if (sessions.IsNullOrEmpty()) return;

            AccountSessionRepo.Delete(sessions);
            await AccountSessionRepo.SaveChanges();
        }

        public async Task<IdentityTokenDto> GenerateTokenByAccountId(IIdentity identity, Guid accountId, string sessionToken = null)
        {
            var account = await AccountService.GetFirstById(identity, accountId);
            var listPermission = await AccountService.GetListPermission(identity, account.Id);
            var profile = await GetProfileByAccountId(identity, accountId);
            var issuer = Configuration.GetValue<string>("Identity:Issuer");
            var secretKey = Configuration.GetValue<string>("Identity:SecretKey");
            var accessTokenLifetime = Configuration.GetValue<int>("Identity:AccessTokenLifetime");
            var sessionTokenLifetime = Configuration.GetValue<int>("Identity:SessionTokenLifetime");

            if (sessionToken == null)
                sessionToken = await CreateSession(identity, accountId);

            var claims = new List<Claim>
            {
                new Claim(
                    nameof(AccountEntity.Id).ToCamelCase(),
                    profile.AccountId.ToString(),
                    ClaimValueTypes.String),
                new Claim(
                    nameof(AccountEntity.Name).ToCamelCase(),
                    profile.Name,
                    ClaimValueTypes.String),
                new Claim(
                    nameof(AccountEntity.AccessLevel).ToCamelCase(),
                    account.AccessLevel.ToString("D"),
                    ClaimValueTypes.Boolean),
            };

            if (profile.Username.IsNotNullOrWhiteSpace())
            {
                claims.Add(new Claim(
                    nameof(profile.Username).ToCamelCase(),
                    profile.Username,
                    ClaimValueTypes.String));
            }

            if (profile.Email.IsNotNullOrWhiteSpace())
            {
                claims.Add(new Claim(
                    nameof(profile.Email).ToCamelCase(),
                    profile.Email,
                    ClaimValueTypes.String));
            }

            if (profile.AvatarLink.IsNotNullOrWhiteSpace())
            {
                claims.Add(new Claim(
                    nameof(profile.AvatarLink).ToCamelCase(),
                    profile.AvatarLink,
                    ClaimValueTypes.String));
            }

            if (account.AccessLevel != AccountLevels.Root)
            {
                claims.Add(new Claim(
                    "Permission".ToCamelCase(),
                    string.Join(" ", listPermission.Select(x => x.Code)),
                    ClaimValueTypes.String));
            }

            var accessToken = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                issuer,
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(accessTokenLifetime),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)), SecurityAlgorithms.HmacSha256)));

            return new IdentityTokenDto
            {
                AccessToken = accessToken,
                AccessTokenLifetime = accessTokenLifetime,
                RefreshToken = sessionToken,
                RefreshTokenLifetime = sessionTokenLifetime,
                Profile = profile,
            };
        }

        public async Task<IdentityTokenDto> GenerateTokenBySessionToken(IIdentity identity, string sessionToken, bool extendSession = false)
        {
            var session = await AccountSessionRepo.Query
                .WhereNotDeleted()
                .Where(x => x.Code == sessionToken)
                .FirstOrDefaultAsync<AccountSessionEntity>(Mapper);

            if (session == null || DateTime.UtcNow > session.ExpiryDatetime)
            {
                if (session != null)
                {
                    AccountSessionRepo.Delete(session);
                    await AccountSessionRepo.SaveChanges();
                }

                throw new AppException(AppExceptions.SessionInvalid);
            }

            if (extendSession) await ExtendSession(identity, session.Id);
            return await GenerateTokenByAccountId(identity, session.AccountId, session.Code);
        }

        public async Task<IdentityProfileDto> GetProfileByAccountId(IIdentity identity, Guid accountId)
        {
            return await AccountService.GetFirstById<IdentityProfileDto>(identity, accountId, false);
        }

        public async Task<IdentityTokenDto> LoginByUsernamePassword(IIdentity identity, IdentityLoginUsernamePasswordDto input)
        {
            var account = await AccountService.GetByUsernamePassword<AccountEntity>(identity, input.Username, input.Password);
            if (account == null)
                throw new AppException(AppExceptions.AccountAuthInvalid);
            if (account.AccessLevel == AccountLevels.Common && !account.IsEmailVerified)
            {
                await AccountService.GenerateEmailVerify(identity, account.Id, true);
                throw new AppException(AppExceptions.AccountNeedVerifedAndWeAlreadySentVerify);
            }

            return await GenerateTokenByAccountId(identity, account.Id);
        }
    }
}
