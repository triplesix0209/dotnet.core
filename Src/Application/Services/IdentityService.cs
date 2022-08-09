using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Sample.Application.Services
{
    public interface IIdentityService : IService
    {
        Task<IdentityRegisterResultDto> Register(IdentityRegisterInputDto input);

        Task<IdentityTokenDto> LoginByUsernamePassword(string username, string password);

        Task<IdentityProfileDto> GetProfileByAccountId(Guid accountId);

        Task<IdentityTokenDto> RefreshToken(string sessionCode);

        Task ClearSession(Guid accountId);

        Task ClearAllExpiredSession();
    }

    public class IdentityService : BaseService, IIdentityService
    {
        public IApplicationDbContext Db { get; set; }

        public IdentityContext IdentityContext { get; set; }

        public IdentityAppsetting IdentityAppsetting { get; set; }

        public IAccountService AccountService { get; set; }

        public IPermissionService PermissionService { get; set; }

        public ISettingService SettingService { get; set; }

        public async Task<IdentityRegisterResultDto> Register(IdentityRegisterInputDto input)
        {
            input.Username = input.Username.Trim();

            var usernameExisted = await AccountService.AnyByUsername(input.Username, true);
            if (usernameExisted)
                throw new ValueExistedException(nameof(input.Username));

            var autoAccept = await SettingService.GetValue<bool>(x => x.AccountRegisterAutoAccept);
            var account = new Account
            {
                Name = input.Name,
                AccessLevel = AccountLevels.Common,
                IsDeleted = !autoAccept,
                Auths = new List<AccountAuth>(),
            };

            var hashPasswordKey = RandomHelper.RandomString(10);
            var auth = new AccountAuth
            {
                Username = input.Username,
                HashPasswordKey = hashPasswordKey,
                HashedPassword = PasswordHelper.HashPassword(input.Password, hashPasswordKey),
            };
            account.Auths.Add(auth);

            account = await AccountService.Create(account);

            var result = new IdentityRegisterResultDto();
            result.IsAccountActivated = !account.IsDeleted;
            result.Message = result.IsAccountActivated
                ? "Tài khoản đã tạo thành công, hãy bắt đầu đăng nhập"
                : "Tài khoản đã tạo nhưng tạm khóa, xin vui lòng liên hệ quản trị để kích hoạt";
            return result;
        }

        public async Task<IdentityTokenDto> LoginByUsernamePassword(string username, string password)
        {
            var account = await AccountService.GetByUsernamePassword<Account>(username, password, false);
            if (account == null || account.IsDeleted)
                throw new AccountNotFoundException("Không tìm thấy tài khoản, xin vui lòng kiểm tra lại thông tin đăng nhập");

            return await GenerateAccessToken(account);
        }

        public async Task<IdentityProfileDto> GetProfileByAccountId(Guid accountId)
        {
            var account = await AccountService.GetById(accountId, false);
            return await GetProfileByAccount(account);
        }

        public async Task<IdentityTokenDto> RefreshToken(string sessionCode)
        {
            var now = DateTime.UtcNow;
            var session = await Db.AccountSession
                .WhereNotDeleted()
                .Where(x => x.Code == sessionCode)
                .Where(x => now < x.ExpiryDatetime)
                .FirstOrDefaultAsync();
            if (session == null)
                throw new SessionInvalidException();

            await ExtendSession(session);
            return await GenerateAccessToken(session.Account, session);
        }

        public async Task ClearSession(Guid accountId)
        {
            var sessions = await Db.AccountSession
                .Where(x => x.AccountId == accountId)
                .ToListAsync();
            if (sessions.IsNullOrEmpty()) return;

            Db.AccountSession.RemoveRange(sessions);
            await Db.SaveChangesAsync();
        }

        public async Task ClearAllExpiredSession()
        {
            var sessions = await Db.AccountSession
                .Where(x => DateTime.UtcNow > x.ExpiryDatetime)
                .ToListAsync();

            if (sessions.IsNullOrEmpty()) return;

            Db.AccountSession.RemoveRange(sessions);
            await Db.SaveChangesAsync();
        }

        protected Task<IdentityProfileDto> GetProfileByAccount(Account account)
        {
            var profile = new IdentityProfileDto
            {
                AccountId = account.Id,
                Name = account.Name,
                Username = account.Auths.FirstOrDefault()?.Username,
                AvatarLink = account.AvatarLink,
            };

            return Task.FromResult(profile);
        }

        protected async Task<IdentityTokenDto> GenerateAccessToken(Account account, AccountSession? session = null)
        {
            var profile = await GetProfileByAccount(account);
            if (session == null) session = await GenerateSession(account);

            var permissions = account.AccessLevel != AccountLevels.Root && account.PermissionGroupId.HasValue
                ? (await PermissionService.GetListPermissionValue(account.PermissionGroupId.Value, true)).Select(x => x.Code)
                : null;

            var accessToken = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                IdentityAppsetting.Issuer,
                claims: IdentityContext.GenerateClaim(profile, (int)account.AccessLevel, x => x.AccountId, permissions),
                expires: DateTime.UtcNow.AddSeconds(IdentityAppsetting.AccessTokenLifetime),
                signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(
                    new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(IdentityAppsetting.SecretKey)),
                    Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256)));

            return new IdentityTokenDto
            {
                AccessToken = accessToken,
                AccessTokenLifetime = IdentityAppsetting.AccessTokenLifetime,
                RefreshToken = session.Code,
                RefreshTokenLifetime = IdentityAppsetting.SessionTokenLifetime,
                Profile = profile,
            };
        }

        protected async Task<AccountSession> GenerateSession(Account account)
        {
            var session = await Db.AccountSession
                .WhereNotDeleted()
                .Where(x => x.AccountId == account.Id)
                .FirstOrDefaultAsync();

            if (session != null)
            {
                await ExtendSession(session);
                return session;
            }

            session = new AccountSession
            {
                AccountId = account.Id,
                Code = Guid.NewGuid().ToString().Replace(@"-", string.Empty),
                ExpiryDatetime = DateTime.UtcNow.AddSeconds(IdentityAppsetting.SessionTokenLifetime),
            };

            Db.AccountSession.Add(session);
            await Db.SaveChangesAsync();
            return session;
        }

        protected async Task ExtendSession(AccountSession session)
        {
            session.ExpiryDatetime = DateTime.UtcNow.AddSeconds(IdentityAppsetting.SessionTokenLifetime);
            Db.AccountSession.Update(session);
            await Db.SaveChangesAsync();
        }
    }
}
