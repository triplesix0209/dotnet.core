using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TripleSix.Core.Attributes;
using TripleSix.Core.Dto;
using TripleSix.Core.Entities;
using TripleSix.Core.Services;
using TripleSix.Core.WebApi.Filters;
using TripleSix.Core.WebApi.Results;

namespace TripleSix.Core.AutoAdmin
{
    public abstract class BaseAdminControllerCreateMethod<TEntity, TCreateDto>
        : BaseAdminController
        where TEntity : class, IModelEntity
        where TCreateDto : class, IDataDto
    {
        public ICommonService<TEntity> Service { get; set; }

        [HttpPost]
        [SwaggerApi("tạo [controller]", typeof(DataResult<Guid>))]
        [AdminMethod(Type = AdminMethodTypes.Create)]
        [PermissionRequirement(AutoGroup = true, ListCode = new[] { "create" })]
        [Transactional]
        public async Task<IActionResult> Create([FromBody] TCreateDto input)
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
