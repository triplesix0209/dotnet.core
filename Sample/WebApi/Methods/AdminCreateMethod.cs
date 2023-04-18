using TripleSix.Core.Entities;
using TripleSix.Core.Types;

namespace Sample.WebApi.Methods
{
    [Route("Admin/[controller]")]
    [ApiExplorerSettings(GroupName = "admin")]
    [Authorize]
    public class AdminCreateMethod<TEntity, TAdminModel, TCreateDto>
        : BaseAdminControllerCreateMethod<TEntity, TAdminModel, TCreateDto>
        where TEntity : class, IStrongEntity
        where TAdminModel : AdminModel<TEntity>
        where TCreateDto : class, IDto
    {
    }
}
