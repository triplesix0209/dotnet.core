using TripleSix.Core.Entities;
using TripleSix.Core.Types;

namespace Sample.WebApi.Controllers.Admins.Auto
{
    [Route("Admin/[controller]")]
    [ApiExplorerSettings(GroupName = "admin")]
    [Authorize]
    public class AdminControllerReadMethod<TAdminDto, TEntity, TFilterDto, TItemDto, TDetailDto>
        : BaseAdminControllerReadMethod<TAdminDto, TEntity, TFilterDto, TItemDto, TDetailDto>
        where TAdminDto : IAdminModel
        where TEntity : class, IStrongEntity
        where TFilterDto : PagingInputDto, IQueryableDto<TEntity>
        where TItemDto : class, IDto
        where TDetailDto : class, IDto
    {
    }
}
