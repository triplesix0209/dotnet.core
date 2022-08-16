using TripleSix.Core.Entities;
using TripleSix.Core.Types;

namespace Sample.WebApi.Common
{
    [Route("Admin/[controller]")]
    [ApiExplorerSettings(GroupName = "admin")]
    [Authorize]
    [AccessLevelRequirement(MinimumAccountLevel = (int)AccountLevels.Admin)]
    public class AdminControllerCreateMethod<TEntity, TAdminModel, TCreateDto>
        : BaseAdminControllerCreateMethod<TEntity, TAdminModel, TCreateDto>
        where TEntity : class, IStrongEntity
        where TAdminModel : AdminModel<TEntity>
        where TCreateDto : class, IDto
    {
    }
}
