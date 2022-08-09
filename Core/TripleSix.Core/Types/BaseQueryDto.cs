using TripleSix.Core.Entities;
using TripleSix.Core.Persistences;

namespace TripleSix.Core.Types
{
    /// <summary>
    /// DTO query.
    /// </summary>
    /// <typeparam name="TEntity">Loại entity.</typeparam>
    public abstract class BaseQueryDto<TEntity> : BaseDto,
        IQueryableModel<TEntity>
        where TEntity : IEntity
    {
        /// <inheritdoc/>
        public abstract IQueryable<TEntity> ToQueryable(IDbDataContext db);
    }
}
