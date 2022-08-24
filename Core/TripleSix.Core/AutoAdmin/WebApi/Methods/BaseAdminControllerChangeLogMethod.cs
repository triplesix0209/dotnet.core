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

        [HttpGet("{id}/ChangeLog")]
        [SwaggerOperation("Lấy danh sách thay đổi [controller]")]
        [AdminMethod(Type = AdminMethodTypes.ListChangeLog)]
        public virtual async Task<PagingResult<ChangeLogItemDto>> GetPageChangeLog(RouteId route, PagingInputDto filter)
        {
            var result = await ObjectLogService.GetPageObjectLog(typeof(TEntity).Name, route.Id, filter.Page, filter.Size);
            return PagingResult(result);
        }

        [HttpGet("{id}/ChangeLog/{logId}")]
        [SwaggerOperation("Lấy chi tiết thay đổi [controller]")]
        [AdminMethod(Type = AdminMethodTypes.DetailChangeLog)]
        public virtual async Task<DataResult<ChangeLogDetailDto>> GetDetailChangeLog(RouteId route, RouteLog routeLog)
        {
            var result = await ObjectLogService.GetDetailObjectLog(typeof(TEntity).Name, route.Id, routeLog.LogId);
            return DataResult(result);
        }
    }
}
