using System;
using System.Threading.Tasks;
using Sample.Common.Dto;
using Sample.Data.Entities;

namespace Sample.Middle.Services
{
    public interface IPermissionGroupService : ICommonService<PermissionGroupEntity>,
        IReadableWithModel<PermissionGroupEntity, PermissionGroupAdminDto.Detail>,
        ICreatableWithModel<PermissionGroupAdminDto.Create>,
        IUpdatableWithModel<PermissionGroupAdminDto.Update>
    {
        Task<PermissionValueDto[]> GetListPermissionValue(IIdentity identity, Guid? id = null);
    }
}
