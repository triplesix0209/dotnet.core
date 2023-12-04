using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sample.Common;
using Sample.Common.Dto;
using Sample.Common.Enum;
using Sample.Common.Helpers;
using Sample.Data.Entities;
using Sample.Data.Repositories;
using Sample.Middle.Abstracts;
using Sample.Middle.Helpers;
using TripleSix.CoreOld.Dto;
using TripleSix.CoreOld.Helpers;
using TripleSix.CoreOld.Mappers;

namespace Sample.Middle.Services
{
    public class AccountService : CommonService<AccountEntity, AccountAdminDto>,
        IAccountService
    {
        public AccountService(AccountRepository repo)
            : base(repo)
        {
        }

        public AccountRepository AccountRepo { get; set; }

        public AccountAuthRepository AccountAuthRepo { get; set; }

        public AccountVerifyRepository AccountVerifyRepo { get; set; }

        public ISettingService SettingService { get; set; }

        public IPermissionGroupService PermissionGroupService { get; set; }

        public MailHelper MailHelper { get; set; }

        public async Task CreateAuthUsernamePassword(IIdentity identity, Guid accountId, string username, string password)
        {
            var account = await GetFirstById(identity, accountId);
            if (account.Auths != null && account.Auths.Any(x => x.Type == AccountAuthTypes.UsernamePassword))
                throw new AppException(AppExceptions.AccountAuthExisted, args: AccountAuthTypes.UsernamePassword.GetDescription());
            if (account.Auths == null) account.Auths = new List<AccountAuthEntity>();

            var hashPasswordKey = RandomHelper.RandomString(10);
            account.Auths.Add(new AccountAuthEntity
            {
                Type = AccountAuthTypes.UsernamePassword,
                HashPasswordKey = hashPasswordKey,
                Username = username,
                HashedPassword = PasswordHelper.Hash(hashPasswordKey, password),
            });

            AccountRepo.Update(account);
            await SaveChanges();
        }

        public async Task<TResult> CreateWithModel<TResult>(IIdentity identity, AccountAdminDto.Create input, bool autoGenerateCode = true)
            where TResult : class
        {
            if (input.AccessLevel == AccountLevels.Root)
                throw new AppException(AppExceptions.CannotCreateRootAccount);

            input.Username = input.Username.Trim().ToLower();
            if (input.Email.IsNullOrWhiteSpace())
                input.IsEmailVerified = false;

            if (await GetByUsername<AccountEntity>(identity, input.Username) != null)
                throw new AppException(AppExceptions.UsernameIsExisted, args: input.Username);

            var account = await CreateWithMapper<AccountEntity>(identity, input, autoGenerateCode);
            AccountAuthRepo.Create(new AccountAuthEntity
            {
                AccountId = account.Id,
                Type = AccountAuthTypes.UsernamePassword,
                Username = input.Username,
                HashPasswordKey = RandomHelper.RandomString(10),
            });

            await SaveChanges();
            await SetPassword(identity, account.Id, input.Password);
            return Mapper.Map<TResult>(account);
        }

        public async Task<TResult> CreateWithModel<TResult>(IIdentity identity, IdentityRegisterDto input, bool autoGenerateCode = true)
            where TResult : class
        {
            input.Username = input.Username.Trim().ToLower();
            if (await GetByUsername<AccountEntity>(identity, input.Username) != null)
                throw new AppException(AppExceptions.UsernameIsExisted, args: input.Username);

            var account = await Create(
                identity,
                new AccountEntity
                {
                    Name = input.Name,
                    Email = input.Email,
                    IsEmailVerified = false,
                    AccessLevel = AccountLevels.Common,
                },
                autoGenerateCode);
            AccountAuthRepo.Create(new AccountAuthEntity
            {
                AccountId = account.Id,
                Type = AccountAuthTypes.UsernamePassword,
                Username = input.Username,
                HashPasswordKey = RandomHelper.RandomString(10),
            });

            await SaveChanges();
            await SetPassword(identity, account.Id, input.Password);
            await GenerateEmailVerify(identity, account.Id, true);
            return Mapper.Map<TResult>(account);
        }

        public async Task UpdateWithModel(IIdentity identity, Guid id, AccountAdminDto.Update input)
        {
            var account = await GetFirstById(identity, id);
            if (account.AccessLevel == AccountLevels.Root)
                throw new AppException(AppExceptions.RootAccountUnmodified);
            if (input.AccessLevel == AccountLevels.Root)
                throw new AppException(AppExceptions.CannotCreateRootAccount);

            if (input.Username.IsNotNullOrWhiteSpace())
                input.Username = input.Username.Trim().ToLower();
            if (input.Email.IsNullOrWhiteSpace())
                input.IsEmailVerified = false;

            var auth = await AccountAuthRepo.Query
                .Where(x => x.AccountId == id)
                .Where(x => x.AccountId == id && x.Type == AccountAuthTypes.UsernamePassword)
                .FirstAsync();

            if (input.Username != auth.Username && await GetByUsername<AccountEntity>(identity, input.Username) != null)
                throw new AppException(AppExceptions.UsernameIsExisted, args: input.Username);

            await UpdateWithMapper(identity, id, input);

            if (input.Username.IsNotNullOrWhiteSpace())
            {
                if (auth == null)
                {
                    AccountAuthRepo.Create(new AccountAuthEntity
                    {
                        AccountId = id,
                        Type = AccountAuthTypes.UsernamePassword,
                        Username = input.Username,
                        HashPasswordKey = RandomHelper.RandomString(10),
                    });
                }
                else if (input.Username != auth.Username)
                {
                    auth.Username = input.Username;
                    AccountAuthRepo.Update(auth);
                }
            }

            await SaveChanges();
            if (input.Password.IsNotNullOrWhiteSpace())
                await SetPassword(identity, id, input.Password);
        }

        public override Task Update(IIdentity identity, AccountEntity entity, Action<AccountEntity> @delegate)
        {
            if (entity.AccessLevel == AccountLevels.Root)
                throw new AppException(AppExceptions.RootAccountUnmodified);

            return base.Update(identity, entity, @delegate);
        }

        public Task<TResult> GetByUsername<TResult>(IIdentity identity, string username)
            where TResult : class
        {
            var query =
                from account in AccountRepo.Query
                join auth in AccountAuthRepo.Query
                    on account.Id equals auth.AccountId
                where auth.Type == AccountAuthTypes.UsernamePassword && auth.Username == username
                select account;

            return GetFirstOrDefault<TResult>(identity, query);
        }

        public async Task<TResult> GetByUsernamePassword<TResult>(IIdentity identity, string username, string password)
            where TResult : class
        {
            var auth = await AccountAuthRepo.Query
                .Where(x => x.Type == AccountAuthTypes.UsernamePassword && x.Username == username)
                .FirstOrDefaultAsync<AccountAuthEntity>(Mapper);

            if (auth == null || PasswordHelper.Check(auth.HashPasswordKey, password, auth.HashedPassword) == false)
                return default;

            return Mapper.MapData<AccountEntity, TResult>(auth.Account);
        }

        public Task<TResult> GetByEmail<TResult>(IIdentity identity, string email)
            where TResult : class
        {
            return GetFirstOrDefault<TResult>(identity, AccountRepo.Query
                .Where(x => x.Email == email));
        }

        public async Task SetPassword(IIdentity identity, Guid accountId, string newPassword)
        {
            if (newPassword.IsNullOrWhiteSpace() || newPassword.Length < 6)
                throw new AppException(AppExceptions.PasswordTooWeak);

            var account = await GetFirstById(identity, accountId);
            var auths = account.Auths.Where(x => x.Type == AccountAuthTypes.UsernamePassword);

            if (!auths.Any())
                throw new AppException(AppExceptions.AccountAuthInactiveOrNotFound);

            foreach (var auth in auths)
                auth.HashedPassword = PasswordHelper.Hash(auth.HashPasswordKey, newPassword);

            AccountAuthRepo.Update(auths);
            await SaveChanges();
        }

        public async Task UpdatePassword(IIdentity identity, Guid accountId, string oldPassword, string newPassword)
        {
            var auths = await AccountAuthRepo.Query
                .WhereNotDeleted()
                .Where(x => x.AccountId == accountId && x.Type == AccountAuthTypes.UsernamePassword)
                .ToArrayAsync<AccountAuthEntity>(Mapper);

            if (!auths.Any(x => PasswordHelper.Check(x.HashPasswordKey, oldPassword, x.HashedPassword)))
                throw new AppException(AppExceptions.OldPasswordInvalid);

            await SetPassword(identity, accountId, newPassword);
        }

        public async Task<PermissionValueDto[]> GetListPermission(IIdentity identity, Guid accountId)
        {
            var account = await GetFirstById(identity, accountId);
            if (account.AccessLevel == AccountLevels.Root) return null;

            var permissions = await PermissionGroupService.GetListPermissionValue(identity, account.PermissionGroupId);
            return permissions.Where(x => x.ActualValue == true).ToArray();
        }

        public async Task<bool> CheckPermission(IIdentity identity, Guid accountId, string permissionCode)
        {
            var permissions = await GetListPermission(identity, accountId);
            if (permissions == null) return true;
            return permissions.Any(x => x.Code == permissionCode);
        }

        public async Task<AccountVerifyEntity> GenerateResetPasswordVerify(IIdentity identity, Guid accountId, bool resendAsPossible)
        {
            var account = await GetFirstById(identity, accountId);
            if (account.Email.IsNullOrWhiteSpace())
                throw new AppException(AppExceptions.AccountEmailInvalid);
            if (!account.IsEmailVerified)
                throw new AppException(AppExceptions.AccountNeedVerifed);

            AccountVerifyEntity verify = null;
            if (resendAsPossible)
                verify = await GetVerifyByAccount(identity, accountId, AccountVerifyTypes.ResetPassword);

            if (verify is null)
            {
                var timelife = await SettingService.GetValue<int>(identity, "resetPassword.timelife");
                verify = new AccountVerifyEntity
                {
                    AccountId = accountId,
                    Type = AccountVerifyTypes.ResetPassword,
                    ExpiryDatetime = DateTime.Now.AddMinutes(timelife),
                };

                AccountVerifyRepo.Create(verify);
                await AccountVerifyRepo.SaveChanges();
            }

            var verifyLink = await SettingService.GetValue(identity, "resetPassword.verifyLink");
            var emailSubject = await SettingService.GetValue(identity, "resetPassword.emailSubject");
            var emailBody = await SettingService.GetValue(identity, "resetPassword.emailBody");
            verifyLink = string.Format(verifyLink, verify.Id.ToString());
            emailBody = string.Format(emailBody, verifyLink);
            await MailHelper.SendMail(account.Email, emailSubject, emailBody);

            return verify;
        }

        public async Task<AccountVerifyEntity> GenerateEmailVerify(IIdentity identity, Guid accountId, bool resendAsPossible)
        {
            var account = await GetFirstById(identity, accountId);
            if (account.Email.IsNullOrWhiteSpace())
                throw new AppException(AppExceptions.AccountEmailInvalid);
            if (account.IsEmailVerified)
                throw new AppException(AppExceptions.AccountAlreadyVerifed);

            AccountVerifyEntity verify = null;
            if (resendAsPossible)
                verify = await GetVerifyByAccount(identity, accountId, AccountVerifyTypes.Email);

            if (verify is null)
            {
                var timelife = await SettingService.GetValue<int>(identity, "accountVerify.timelife");
                verify = new AccountVerifyEntity
                {
                    AccountId = accountId,
                    Type = AccountVerifyTypes.Email,
                    ExpiryDatetime = DateTime.Now.AddMinutes(timelife),
                };

                AccountVerifyRepo.Create(verify);
                await AccountVerifyRepo.SaveChanges();
            }

            var verifyLink = await SettingService.GetValue(identity, "accountVerify.verifyLink");
            var emailSubject = await SettingService.GetValue(identity, "accountVerify.emailSubject");
            var emailBody = await SettingService.GetValue(identity, "accountVerify.emailBody");
            verifyLink = string.Format(verifyLink, verify.Id.ToString());
            emailBody = string.Format(emailBody, verifyLink);
            await MailHelper.SendMail(account.Email, emailSubject, emailBody);

            return verify;
        }

        public async Task<AccountVerifyEntity> Verify(IIdentity identity, Guid verifyId)
        {
            await ClearVerifyExpired(identity);
            var verify = await AccountVerifyRepo.Query
                .WhereNotDeleted()
                .Where(x => x.Id == verifyId)
                .FirstOrDefaultAsync<AccountVerifyEntity>(Mapper);

            if (verify is not null)
            {
                AccountVerifyRepo.Delete(verify);
                await AccountVerifyRepo.SaveChanges();

                await Update(identity, verify.AccountId, (e) =>
                {
                    if (verify.Type == AccountVerifyTypes.Email)
                        e.IsEmailVerified = true;
                });
            }

            return verify;
        }

        public async Task ClearVerifyExpired(IIdentity identity)
        {
            var items = await AccountVerifyRepo.Query
                .Where(x => DateTime.UtcNow > x.ExpiryDatetime)
                .ToArrayAsync<AccountVerifyEntity>(Mapper);

            if (items.IsNullOrEmpty()) return;

            AccountVerifyRepo.Delete(items);
            await AccountVerifyRepo.SaveChanges();
        }

        private async Task<AccountVerifyEntity> GetVerifyByAccount(IIdentity identity, Guid accountId, AccountVerifyTypes type)
        {
            return await AccountVerifyRepo.Query
                    .WhereNotDeleted()
                    .Where(x => x.AccountId == accountId)
                    .Where(x => x.Type == type)
                    .Where(x => x.ExpiryDatetime < DateTime.Now)
                    .FirstOrDefaultAsync<AccountVerifyEntity>(Mapper);
        }
    }
}
