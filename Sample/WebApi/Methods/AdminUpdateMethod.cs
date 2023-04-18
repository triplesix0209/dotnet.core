using TripleSix.Core.Entities;
using TripleSix.Core.Types;

namespace Sample.WebApi.Methods
{
    [Route("Admin/[controller]")]
    [ApiExplorerSettings(GroupName = "admin")]
    [Authorize]
    public class AdminUpdateMethod<TEntity, TAdminModel, TUpdateDto>
        : BaseAdminControllerUpdateMethod<TEntity, TAdminModel, TUpdateDto>
        where TEntity : class, IStrongEntity
        where TAdminModel : AdminModel<TEntity>
        where TUpdateDto : class, IDto
    {
    }
}
