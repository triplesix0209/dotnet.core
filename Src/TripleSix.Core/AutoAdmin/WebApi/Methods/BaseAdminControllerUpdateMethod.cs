using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TripleSix.Core.Attributes;
using TripleSix.Core.Dto;
using TripleSix.Core.Entities;
using TripleSix.Core.Services;
using TripleSix.Core.WebApi.Filters;

namespace TripleSix.Core.AutoAdmin
{
    public abstract class BaseAdminControllerUpdateMethod<TEntity, TUpdateDto>
        : BaseAdminController
        where TEntity : class, IModelEntity
        where TUpdateDto : class, IDataDto
    {
        public ICommonService<TEntity> Service { get; set; }

        [HttpPut("{id}")]
        [SwaggerApi("sửa [controller]")]
        [AdminMethod(Type = AdminMethodTypes.Update)]
        [PermissionRequirement(AutoGroup = true, ListCode = new[] { "update" })]
        [Transactional]
        public async Task<IActionResult> Update(RouteId route, [FromBody] TUpdateDto input)
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
