using TripleSix.Core.Entities;

namespace TripleSix.Core.Types
{
    /// <summary>
    /// DTO query.
    /// </summary>
    /// <typeparam name="TEntity">Loại entity.</typeparam>
    public abstract class BaseQueryDto<TEntity> : BaseDto,
        IQueryableDto<TEntity>
        where TEntity : IEntity
    {
        public abstract IQueryable<TEntity> ToQueryable(IQueryable<TEntity> query);
    }
}
