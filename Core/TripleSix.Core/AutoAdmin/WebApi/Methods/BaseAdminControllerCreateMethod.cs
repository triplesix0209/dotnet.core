using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TripleSix.Core.Entities;
using TripleSix.Core.Helpers;
using TripleSix.Core.Services;
using TripleSix.Core.Types;
using TripleSix.Core.WebApi;

namespace TripleSix.Core.AutoAdmin
{
    public abstract class BaseAdminControllerCreateMethod<TEntity, TAdminModel, TCreateDto>
        : BaseAdminController, IAdminMethod
        where TEntity : class, IStrongEntity
        where TAdminModel : AdminModel<TEntity>
        where TCreateDto : class, IDto
    {
        public IStrongService<TEntity> Service { get; set; }

        public IObjectLogService ObjectLogService { get; set; }

        [HttpPost]
        [SwaggerOperation("tạo [controller]")]
        [AdminMethod(Type = AdminMethodTypes.Create)]
        [Transactional]
        public virtual async Task<DataResult<Guid>> Create([FromBody] AdminSubmitDto<TCreateDto> input)
        {
            var result = await Service.CreateWithMapper<TEntity>(input.Data);

            if (ObjectLogService != null)
            {
                if (input.Note.IsNullOrEmpty()) input.Note = "Khởi tạo";
                await ObjectLogService.WriteLog(result.Id, result, note: input.Note);
            }

            return DataResult(result.Id);
        }
    }
}
