using AutoMapper;
using TripleSix.Core.Entities;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Types
{
    /// <summary>
    /// DTO input cơ bản.
    /// </summary>
    /// <typeparam name="TEntity">Loại entity.</typeparam>
    public abstract class BaseInputDto<TEntity> : BaseDto,
        IMapToEntityDto<TEntity>
        where TEntity : class, IEntity
    {
        /// <inheritdoc/>
        public virtual Task<TEntity> MapToEntity(IMapper mapper, IServiceProvider serviceProvider)
        {
            var result = mapper.MapData<TEntity>(this);
            return Task.FromResult(result);
        }

        /// <inheritdoc/>
        public virtual Task<TEntity> MapChangeEntity(IMapper mapper, IServiceProvider serviceProvider, TEntity sourceEntity)
        {
            var result = mapper.MapUpdate<TEntity>(this, sourceEntity);
            return Task.FromResult(result);
        }
    }
}
