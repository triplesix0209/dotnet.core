using TripleSix.Core.Entities;
using TripleSix.Core.Types;

namespace Sample.WebApi.Common.AdminAutoMethods
{
    [Route("Admin/[controller]")]
    [ApiExplorerSettings(GroupName = "admin")]
    [Authorize]
    public class AdminControllerReadMethod<TEntity, TAdminModel, TFilterDto, TItemDto, TDetailDto>
        : BaseAdminControllerReadMethod<TEntity, TAdminModel, TFilterDto, TItemDto, TDetailDto>
        where TEntity : class, IStrongEntity
        where TAdminModel : AdminModel<TEntity>
        where TFilterDto : PagingInputDto, IQueryableDto<TEntity>
        where TItemDto : class, IDto
        where TDetailDto : class, IDto
    {
    }
}
