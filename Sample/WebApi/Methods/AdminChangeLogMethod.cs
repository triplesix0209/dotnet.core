using TripleSix.Core.Entities;

namespace Sample.WebApi.Methods
{
    [Route("Admin/[controller]")]
    [ApiExplorerSettings(GroupName = "admin")]
    [Authorize]
    public class AdminChangeLogMethod<TEntity, TAdminModel>
        : BaseAdminControllerChangeLogMethod<TEntity, TAdminModel>
        where TEntity : class, IStrongEntity
        where TAdminModel : AdminModel<TEntity>
    {
    }
}
