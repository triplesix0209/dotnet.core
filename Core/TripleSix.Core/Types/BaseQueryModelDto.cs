using TripleSix.Core.Entities;
using TripleSix.Core.Persistences;

namespace TripleSix.Core.Types
{
    /// <summary>
    /// DTO query.
    /// </summary>
    /// <typeparam name="TEntity">Loại entity.</typeparam>
    /// <typeparam name="TDbContext"><see cref="IDbDataContext"/>.</typeparam>
    public abstract class BaseQueryModelDto<TEntity, TDbContext> : BaseDto,
        IQueryModel<TEntity, TDbContext>
        where TEntity : class, IEntity
        where TDbContext : IDbDataContext
    {
        /// <inheritdoc/>
        public abstract IQueryable<TEntity> ToQueryable(TDbContext db);
    }
}
