using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TripleSix.Core.Entities;
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

        [HttpPost]
        [SwaggerOperation("tạo [controller]")]
        [AdminMethod(Type = AdminMethodTypes.Create)]
        public virtual async Task<DataResult<Guid>> Create([FromBody] TCreateDto input)
        {
            var result = await Service.CreateWithMapper<TEntity>(input);
            return DataResult(result.Id);
        }
    }
}
