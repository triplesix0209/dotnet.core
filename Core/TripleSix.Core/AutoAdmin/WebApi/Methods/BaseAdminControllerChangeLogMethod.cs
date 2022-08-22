using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TripleSix.Core.Entities;
using TripleSix.Core.Types;
using TripleSix.Core.WebApi;

namespace TripleSix.Core.AutoAdmin
{
    public abstract class BaseAdminControllerChangeLogMethod<TEntity, TAdminModel>
        : BaseAdminController, IAdminMethod
        where TEntity : class, IStrongEntity
        where TAdminModel : AdminModel<TEntity>
    {
        public IObjectLogService ObjectLogService { get; set; }

        [HttpGet("ChangeLog")]
        [SwaggerOperation("lấy danh sách thay đổi [controller]")]
        [AdminMethod(Type = AdminMethodTypes.ListChangeLog)]
        public virtual async Task<PagingResult<ChangeLogItemDto>> GetPageChangeLog(PagingInputDto filter)
        {
            var result = await ObjectLogService.GetPageObjectLog(typeof(TEntity).Name, filter.Page, filter.Size);
            return PagingResult(result);
        }
    }
}
