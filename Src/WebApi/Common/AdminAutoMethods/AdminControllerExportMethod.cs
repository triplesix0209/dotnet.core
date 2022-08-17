using TripleSix.Core.Entities;

namespace Sample.WebApi.Common
{
    [Route("Admin/[controller]")]
    [ApiExplorerSettings(GroupName = "admin")]
    [Authorize]
    [AccessLevelRequirement(MinimumAccountLevel = (int)AccountLevels.Admin)]
    public class AdminControllerExportMethod<TEntity, TAdminModel, TFilterDto, TDetailDto>
        : BaseAdminControllerExportMethod<TEntity, TAdminModel, TFilterDto, TDetailDto>
        where TEntity : class, IStrongEntity
        where TAdminModel : AdminModel<TEntity>
        where TFilterDto : BaseAdminFilterDto<TEntity>
        where TDetailDto : BaseAdminItemDto
    {
    }
}
