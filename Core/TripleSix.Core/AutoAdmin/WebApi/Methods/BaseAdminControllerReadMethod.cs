using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TripleSix.Core.Entities;
using TripleSix.Core.Services;
using TripleSix.Core.Types;
using TripleSix.Core.WebApi;

namespace TripleSix.Core.AutoAdmin
{
    public abstract class BaseAdminControllerReadMethod<TAdminDto, TEntity, TFilterDto, TItemDto, TDetailDto>
        : BaseAdminController
        where TAdminDto : IAdminModel
        where TEntity : class, IStrongEntity
        where TFilterDto : PagingInputDto, IQueryableDto<TEntity>
        where TItemDto : class, IDto
        where TDetailDto : class, IDto
    {
        public IStrongService<TEntity> Service { get; set; }

        [HttpGet]
        [SwaggerOperation("lấy danh sách [controller]")]
        [AdminMethod(Type = AdminMethodTypes.List)]
        public virtual async Task<PagingResult<TItemDto>> GetPage(TFilterDto input)
        {
            var result = await Service.GetPageByQueryModel<TItemDto>(input, input.Page, input.Size);
            return PagingResult(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation("lấy chi tiết [controller]")]
        [AdminMethod(Type = AdminMethodTypes.Detail)]
        public virtual async Task<DataResult<TDetailDto>> GetDetail(RouteId route)
        {
            var result = await Service.GetById<TDetailDto>(route.Id, true);
            return DataResult(result);
        }
    }
}
