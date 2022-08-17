using TripleSix.Core.Entities;
using TripleSix.Core.Types;

namespace Sample.WebApi.Common
{
    [Route("Admin/[controller]")]
    [ApiExplorerSettings(GroupName = "admin")]
    [Authorize]
    [AccessLevelRequirement(MinimumAccountLevel = (int)AccountLevels.Admin)]
    public class AdminControllerUpdateMethod<TEntity, TAdminModel, TUpdateDto>
        : BaseAdminControllerUpdateMethod<TEntity, TAdminModel, TUpdateDto>
        where TEntity : class, IStrongEntity
        where TAdminModel : AdminModel<TEntity>
        where TUpdateDto : class, IDto
    {
    }
}
