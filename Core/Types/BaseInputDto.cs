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
        public virtual Task<TEntity> OnMapToEntity(IMapper mapper, IServiceProvider serviceProvider, TEntity? source)
        {
            var result = source == null
                ? mapper.MapData<TEntity>(this)
                : mapper.MapUpdate<TEntity>(this, source);
            return Task.FromResult(result);
        }
    }
}
