using System;
using System.Threading.Tasks;
using Sample.Common.Dto;
using Sample.Data.Entities;
using TripleSix.CoreOld.AutoAdmin;
using TripleSix.CoreOld.Dto;
using TripleSix.CoreOld.Services;

namespace Sample.Middle.Services
{
    public interface IAccountService : ICommonService<AccountEntity>,
        ICreatableWithModel<AccountAdminDto.Create>,
        ICreatableWithModel<IdentityRegisterDto>,
        IUpdatableWithModel<AccountAdminDto.Update>
    {
        Task CreateAuthUsernamePassword(IIdentity identity, Guid accountId, string username, string password);

        Task<TResult> GetByUsername<TResult>(IIdentity identity, string username)
            where TResult : class;

        Task<TResult> GetByUsernamePassword<TResult>(IIdentity identity, string username, string password)
            where TResult : class;

        Task<TResult> GetByEmail<TResult>(IIdentity identity, string email)
            where TResult : class;

        Task SetPassword(IIdentity identity, Guid accountId, string newPassword);

        Task UpdatePassword(IIdentity identity, Guid accountId, string oldPassword, string newPassword);

        Task<PermissionValueDto[]> GetListPermission(IIdentity identity, Guid accountId);

        Task<bool> CheckPermission(IIdentity identity, Guid accountId, string permissionCode);

        Task<AccountVerifyEntity> GenerateResetPasswordVerify(IIdentity identity, Guid accountId, bool resendAsPossible);

        Task<AccountVerifyEntity> GenerateEmailVerify(IIdentity identity, Guid accountId, bool resendAsPossible);

        Task<AccountVerifyEntity> Verify(IIdentity identity, Guid verifyId);

        Task ClearVerifyExpired(IIdentity identity);
    }
}
