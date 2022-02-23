using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TripleSix.Core.Attributes;
using TripleSix.Core.Dto;
using TripleSix.Core.Entities;
using TripleSix.Core.WebApi.Filters;

namespace TripleSix.Core.AutoAdmin
{
    public abstract class BaseAdminControllerDeleteMethod<TEntity>
        : BaseAdminController
        where TEntity : class, IModelEntity
    {
        public ICommonService<TEntity> Service { get; set; }

        [HttpDelete("{id}")]
        [SwaggerApi("xóa [controller]")]
        [AdminMethod(Type = AdminMethodTypes.Delete)]
        [Transactional]
        public async Task<IActionResult> Delete(RouteId route, [SwaggerParameter("ghi chú chỉnh sửa")] string note)
        {
            var identity = GenerateIdentity();

            var beforeData = await Service.SerializeEntity(identity, await Service.GetFirstById(identity, route.Id));
            await Service.SetAsDelete(identity, route.Id);
            await Service.WriteChangeLog(identity, route.Id, beforeData, note);

            return SuccessResult();
        }

        [HttpPut("{id}/Restore")]
        [SwaggerApi("khôi phục [controller]")]
        [AdminMethod(Type = AdminMethodTypes.Restore)]
        [Transactional]
        public async Task<IActionResult> Restore(RouteId route, [SwaggerParameter("ghi chú chỉnh sửa")] string note)
        {
            var identity = GenerateIdentity();

            var beforeData = await Service.SerializeEntity(identity, await Service.GetFirstById(identity, route.Id));
            await Service.Restore(identity, route.Id);
            await Service.WriteChangeLog(identity, route.Id, beforeData, note);

            return SuccessResult();
        }
    }
}
