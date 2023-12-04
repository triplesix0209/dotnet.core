using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TripleSix.CoreOld.Attributes;
using TripleSix.CoreOld.Dto;
using TripleSix.CoreOld.Entities;
using TripleSix.CoreOld.WebApi.Filters;

namespace TripleSix.CoreOld.AutoAdmin
{
    public abstract class BaseAdminControllerDeleteMethod<TAdmin, TEntity>
        : BaseAdminController
        where TAdmin : class, IAdminDto
        where TEntity : class, IModelEntity
    {
        public ICommonService<TEntity> Service { get; set; }

        [HttpDelete("{id}")]
        [SwaggerApi("xóa [controller]")]
        [AdminMethod(Type = AdminMethodTypes.Delete)]
        [PermissionRequirement(AutoGroup = true, Code = "delete")]
        [Transactional]
        public virtual async Task<IActionResult> Delete(RouteId route)
        {
            var identity = GenerateIdentity();

            var beforeData = await Service.SerializeEntity(identity, await Service.GetFirstById(identity, route.Id));
            await Service.SetAsDelete(identity, route.Id);
            await Service.WriteChangeLog(identity, route.Id, beforeData, note: identity.SubmitNote);

            return SuccessResult();
        }

        [HttpPut("{id}/Restore")]
        [SwaggerApi("khôi phục [controller]")]
        [AdminMethod(Type = AdminMethodTypes.Restore)]
        [PermissionRequirement(AutoGroup = true, Code = "delete")]
        [Transactional]
        public virtual async Task<IActionResult> Restore(RouteId route)
        {
            var identity = GenerateIdentity();

            var beforeData = await Service.SerializeEntity(identity, await Service.GetFirstById(identity, route.Id));
            await Service.Restore(identity, route.Id);
            await Service.WriteChangeLog(identity, route.Id, beforeData, note: identity.SubmitNote);

            return SuccessResult();
        }
    }
}
