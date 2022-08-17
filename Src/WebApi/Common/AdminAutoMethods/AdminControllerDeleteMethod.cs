using TripleSix.Core.Entities;

namespace Sample.WebApi.Common
{
    [Route("Admin/[controller]")]
    [ApiExplorerSettings(GroupName = "admin")]
    [Authorize]
    [AccessLevelRequirement(MinimumAccountLevel = (int)AccountLevels.Admin)]
    public class AdminControllerDeleteMethod<TEntity, TAdminModel>
        : BaseAdminControllerDeleteMethod<TEntity, TAdminModel>
        where TEntity : class, IStrongEntity
        where TAdminModel : AdminModel<TEntity>
    {
    }
}
