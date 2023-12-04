using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TripleSix.CoreOld.Attributes;
using TripleSix.CoreOld.Dto;
using TripleSix.CoreOld.Entities;
using TripleSix.CoreOld.Services;
using TripleSix.CoreOld.WebApi.Filters;
using TripleSix.CoreOld.WebApi.Results;

namespace TripleSix.CoreOld.AutoAdmin
{
    public abstract class BaseAdminControllerCreateMethod<TAdmin, TEntity, TCreateDto>
        : BaseAdminController
        where TAdmin : class, IAdminDto
        where TEntity : class, IModelEntity
        where TCreateDto : class, IDataDto
    {
        public ICommonService<TEntity> Service { get; set; }

        [HttpPost]
        [SwaggerApi("tạo [controller]", typeof(DataResult<Guid>))]
        [AdminMethod(Type = AdminMethodTypes.Create)]
        [PermissionRequirement(AutoGroup = true, ListCode = new[] { "create" })]
        [Transactional]
        public virtual async Task<IActionResult> Create([FromBody] TCreateDto input)
        {
            var identity = GenerateIdentity();
            TEntity data;

            var createInterface = typeof(ICreatableWithModel<>).MakeGenericType(typeof(TCreateDto));
            if (Service.GetType().IsAssignableTo(createInterface))
            {
                var method = createInterface.GetMethod(nameof(ICreatableWithModel<DataDto>.CreateWithModel));
                data = await (Task<TEntity>)method
                    .MakeGenericMethod(typeof(TEntity))
                    .Invoke(Service, new object[] { identity, input, true });
            }
            else
            {
                data = await Service.CreateWithMapper<TEntity>(identity, input);
            }

            await Service.WriteChangeLog(identity, data.Id, note: identity.SubmitNote);
            return DataResult(data.Id);
        }
    }
}
