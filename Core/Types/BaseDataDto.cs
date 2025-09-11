using TripleSix.Core.Entities;

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
        public virtual Task MapFromEntity(TEntity entity, IServiceProvider serviceProvider)
        {
            return Task.CompletedTask;
        }
    }
}
