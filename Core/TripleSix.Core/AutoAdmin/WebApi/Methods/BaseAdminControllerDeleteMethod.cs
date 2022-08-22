using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TripleSix.Core.Entities;
using TripleSix.Core.Helpers;
using TripleSix.Core.Services;
using TripleSix.Core.Types;
using TripleSix.Core.WebApi;

namespace TripleSix.Core.AutoAdmin
{
    public abstract class BaseAdminControllerDeleteMethod<TEntity, TAdminModel>
        : BaseAdminController, IAdminMethod
        where TEntity : class, IStrongEntity
        where TAdminModel : AdminModel<TEntity>
    {
        public IStrongService<TEntity> Service { get; set; }

        public IObjectLogService ObjectLogService { get; set; }

        [HttpDelete("{id}")]
        [SwaggerOperation("xóa [controller]")]
        [AdminMethod(Type = AdminMethodTypes.SoftDelete)]
        [Transactional]
        public virtual async Task<SuccessResult> SoftDelete(RouteId route, AdminSubmitDto input)
        {
            if (ObjectLogService == null)
            {
                await Service.SoftDelete(route.Id);
            }
            else
            {
                if (input.Note.IsNullOrEmpty()) input.Note = "Xóa";

                await ObjectLogService.LogAction(
                    route.Id,
                    typeof(TEntity).Name,
                    await Service.GetById(route.Id, true),
                    async () =>
                    {
                        await Service.SoftDelete(route.Id);
                        TEntity result = await Service.GetById(route.Id, true);
                        return result;
                    },
                    note: input.Note);
            }

            return SuccessResult();
        }

        [HttpPut("{id}/Restore")]
        [SwaggerOperation("khôi phục [controller]")]
        [AdminMethod(Type = AdminMethodTypes.Restore)]
        [Transactional]
        public virtual async Task<SuccessResult> Restore(RouteId route, [FromBody] AdminSubmitDto input)
        {
            if (ObjectLogService == null)
            {
                await Service.Restore(route.Id);
            }
            else
            {
                if (input.Note.IsNullOrEmpty()) input.Note = "Khôi phục";

                await ObjectLogService.LogAction(
                    route.Id,
                    typeof(TEntity).Name,
                    await Service.GetById(route.Id, true),
                    async () =>
                    {
                        await Service.Restore(route.Id);
                        TEntity result = await Service.GetById(route.Id, true);
                        return result;
                    },
                    note: input.Note);
            }

            return SuccessResult();
        }
    }
}
