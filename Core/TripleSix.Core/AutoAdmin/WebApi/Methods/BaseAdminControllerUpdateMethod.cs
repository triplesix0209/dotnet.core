using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TripleSix.Core.Entities;
using TripleSix.Core.Services;
using TripleSix.Core.Types;
using TripleSix.Core.WebApi;

namespace TripleSix.Core.AutoAdmin
{
    public abstract class BaseAdminControllerUpdateMethod<TEntity, TAdminModel, TUpdateDto>
        : BaseAdminController, IAdminMethod
        where TEntity : class, IStrongEntity
        where TAdminModel : AdminModel<TEntity>
        where TUpdateDto : class, IDto
    {
        public IStrongService<TEntity> Service { get; set; }

        [HttpGet("{id}/Update")]
        [SwaggerOperation("lấy các thông tin có thể cập nhật của [controller]")]
        [AdminMethod(Type = AdminMethodTypes.UpdateView)]
        public virtual async Task<DataResult<TUpdateDto>> GetUpdate(RouteId route)
        {
            var result = await Service.GetById<TUpdateDto>(route.Id, true);
            return DataResult(result);
        }

        [HttpPut("{id}")]
        [SwaggerOperation("cập nhật [controller]")]
        [AdminMethod(Type = AdminMethodTypes.Update)]
        public virtual async Task<SuccessResult> Update(RouteId route, [FromBody] TUpdateDto input)
        {
            await Service.UpdateWithMapper(route.Id, true, input);
            return SuccessResult();
        }
    }
}
