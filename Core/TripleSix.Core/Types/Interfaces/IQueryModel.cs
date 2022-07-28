using TripleSix.Core.Entities;
using TripleSix.Core.Persistences;

namespace TripleSix.Core.Types
{
    /// <summary>
    /// Model cho phép chuyển thành <see cref="IQueryable"/>.
    /// </summary>
    /// <typeparam name="TEntity">Loại entity.</typeparam>
    /// <typeparam name="TDbContext"><see cref="IDbDataContext"/>.</typeparam>
    public interface IQueryModel<TEntity, TDbContext>
        where TDbContext : IDbDataContext
        where TEntity : IEntity
    {
        /// <summary>
        /// Chuyển hóa thành <see cref="IQueryable"/>.
        /// </summary>
        /// <param name="db"><see cref="IDbDataContext"/>.</param>
        /// <returns><see cref="IQueryable"/>.</returns>
        IQueryable<TEntity> ToQueryable(TDbContext db);
    }
}
