using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TripleSix.Core.Entities;
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

        [HttpDelete("{id}")]
        [SwaggerOperation("tạm xóa [controller]")]
        [AdminMethod(Type = AdminMethodTypes.SoftDelete)]
        public virtual async Task<SuccessResult> SoftDelete(RouteId route)
        {
            await Service.SoftDelete(route.Id);
            return SuccessResult();
        }

        [HttpPut("{id}/Restore")]
        [SwaggerOperation("khôi phục [controller]")]
        [AdminMethod(Type = AdminMethodTypes.Restore)]
        public virtual async Task<SuccessResult> Restore(RouteId route)
        {
            await Service.Restore(route.Id);
            return SuccessResult();
        }
    }
}
