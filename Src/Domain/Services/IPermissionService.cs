namespace Sample.Domain.Services
{
    public interface IPermissionService : IStrongService<PermissionGroup>
    {
        Task<List<PermissionItemDto>> GetAllPermission();

        Task<List<PermissionValueDto>> GetListPermissionValue(Guid id, bool grantedOnly);
    }
}
