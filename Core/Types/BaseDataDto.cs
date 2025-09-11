using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TripleSix.Core.Entities;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Types
{
    /// <summary>
    /// DTO data cơ bản.
    /// </summary>
    /// <typeparam name="TEntity">Loại entity.</typeparam>
    public abstract class BaseDataDto<TEntity> : BaseDto,
        IMapFromEntityDto<TEntity>
        where TEntity : IEntity
    {
        /// <inheritdoc/>
        public virtual Task OnMapFromEntity(IServiceProvider serviceProvider, TEntity sourceEntity)
        {
            var mapper = serviceProvider.GetRequiredService<IMapper>();
            mapper.MapData(sourceEntity, this);
            return Task.CompletedTask;
        }
    }
}
