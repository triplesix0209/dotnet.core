using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TripleSix.CoreOld.Attributes;
using TripleSix.CoreOld.Dto;
using TripleSix.CoreOld.Entities;
using TripleSix.CoreOld.Services;
using TripleSix.CoreOld.WebApi.Filters;

namespace TripleSix.CoreOld.AutoAdmin
{
    public abstract class BaseAdminControllerUpdateMethod<TAdmin, TEntity, TUpdateDto>
        : BaseAdminController
        where TAdmin : class, IAdminDto
        where TEntity : class, IModelEntity
        where TUpdateDto : class, IDataDto
    {
        public ICommonService<TEntity> Service { get; set; }

        [HttpPut("{id}")]
        [SwaggerApi("sửa [controller]")]
        [AdminMethod(Type = AdminMethodTypes.Update)]
        [PermissionRequirement(AutoGroup = true, ListCode = new[] { "update" })]
        [Transactional]
        public virtual async Task<IActionResult> Update(RouteId route, [FromBody] TUpdateDto input)
        {
            var identity = GenerateIdentity();
            var beforeData = await Service.SerializeEntity(identity, await Service.GetFirstById(identity, route.Id));

            var updateInterface = typeof(IUpdatableWithModel<>).MakeGenericType(typeof(TUpdateDto));
            if (Service.GetType().IsAssignableTo(updateInterface))
            {
                var method = updateInterface.GetMethod(nameof(IUpdatableWithModel<DataDto>.UpdateWithModel));
                await (Task)method.Invoke(Service, new object[] { identity, route.Id, input });
            }
            else
            {
                await Service.UpdateWithMapper(identity, route.Id, input);
            }

            await Service.WriteChangeLog(identity, route.Id, beforeData, note: identity.SubmitNote);
            return SuccessResult();
        }
    }
}
