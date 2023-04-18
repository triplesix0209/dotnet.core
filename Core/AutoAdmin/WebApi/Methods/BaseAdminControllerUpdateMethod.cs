using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TripleSix.Core.Entities;
using TripleSix.Core.Helpers;
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

        public IObjectLogService ObjectLogService { get; set; }

        [HttpGet("{id}/Update")]
        [SwaggerOperation("Lấy các thông tin có thể sửa của [controller]")]
        [AdminMethod(Type = AdminMethodTypes.UpdateView)]
        public virtual async Task<DataResult<TUpdateDto>> GetUpdate(RouteId route)
        {
            var result = await Service.GetById<TUpdateDto>(route.Id, true);
            return DataResult(result);
        }

        [HttpPut("{id}")]
        [SwaggerOperation("Sửa [controller]")]
        [AdminMethod(Type = AdminMethodTypes.Update)]
        [Transactional]
        public virtual async Task<SuccessResult> Update(RouteId route, [FromBody] AdminSubmitDto<TUpdateDto> input)
        {
            if (ObjectLogService == null)
            {
                await Service.UpdateWithMapper(route.Id, true, input.Data);
            }
            else
            {
                if (input.Note.IsNullOrEmpty()) input.Note = "Sửa";

                await ObjectLogService.LogAction(
                    route.Id,
                    typeof(TEntity).Name,
                    await Service.GetById(route.Id, true),
                    async () =>
                    {
                        await Service.UpdateWithMapper(route.Id, true, input.Data);
                        TEntity result = await Service.GetById(route.Id, true);
                        return result;
                    },
                    note: input.Note);
            }

            return SuccessResult();
        }
    }
}
