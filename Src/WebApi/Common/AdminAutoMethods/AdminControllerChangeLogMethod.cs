using TripleSix.Core.Entities;

namespace Sample.WebApi.Common
{
    [Route("Admin/[controller]")]
    [ApiExplorerSettings(GroupName = "admin")]
    [Authorize]
    [AccessLevelRequirement(MinimumAccountLevel = (int)AccountLevels.Admin)]
    public class AdminControllerChangeLogMethod<TEntity, TAdminModel>
        : BaseAdminControllerChangeLogMethod<TEntity, TAdminModel>
        where TEntity : class, IStrongEntity
        where TAdminModel : AdminModel<TEntity>
    {
    }
}
