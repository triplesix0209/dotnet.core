using TripleSix.Core.Entities;

namespace Sample.WebApi.Methods
{
    [Route("Admin/[controller]")]
    [ApiExplorerSettings(GroupName = "admin")]
    [Authorize]
    public class AdminExportMethod<TEntity, TAdminModel, TFilterDto, TDetailDto>
        : BaseAdminControllerExportMethod<TEntity, TAdminModel, TFilterDto, TDetailDto>
        where TEntity : class, IStrongEntity
        where TAdminModel : AdminModel<TEntity>
        where TFilterDto : BaseAdminFilterDto<TEntity>
        where TDetailDto : BaseAdminItemDto
    {
    }
}
