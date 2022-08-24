using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TripleSix.Core.Entities;
using TripleSix.Core.Services;
using TripleSix.Core.Types;
using TripleSix.Core.WebApi;

namespace TripleSix.Core.AutoAdmin
{
    public abstract class BaseAdminControllerReadMethod<TEntity, TAdminModel, TFilterDto, TItemDto, TDetailDto>
        : BaseAdminController, IAdminMethod
        where TEntity : class, IStrongEntity
        where TAdminModel : AdminModel<TEntity>
        where TFilterDto : BaseAdminFilterDto<TEntity>
        where TItemDto : BaseAdminItemDto
        where TDetailDto : BaseAdminItemDto
    {
        public IStrongService<TEntity> Service { get; set; }

        [HttpGet]
        [SwaggerOperation("Lấy danh sách [controller]")]
        [AdminMethod(Type = AdminMethodTypes.List)]
        public virtual async Task<PagingResult<TItemDto>> GetPage(TFilterDto filter)
        {
            var result = await Service.GetPageByQueryModel<TItemDto>(filter, filter.Page, filter.Size);
            return PagingResult(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation("Lấy chi tiết [controller]")]
        [AdminMethod(Type = AdminMethodTypes.Detail)]
        public virtual async Task<DataResult<TDetailDto>> GetDetail(RouteId route)
        {
            var result = await Service.GetById<TDetailDto>(route.Id, true);
            return DataResult(result);
        }
    }
}
