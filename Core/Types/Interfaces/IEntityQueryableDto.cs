using TripleSix.Core.Entities;

namespace TripleSix.Core.Types
{
    /// <summary>
    /// Model cho phép chuyển thành <see cref="IQueryable"/>.
    /// </summary>
    /// <typeparam name="TEntity">Loại entity.</typeparam>
    public interface IEntityQueryableDto<TEntity>
        where TEntity : IEntity
    {
        /// <summary>
        /// Chuyển hóa thành <see cref="IQueryable"/>.
        /// </summary>
        /// <param name="query">Câu query gốc.</param>
        /// <param name="serviceProvider">Service provider.</param>
        /// <returns><see cref="IQueryable"/>.</returns>
        Task<IQueryable<TEntity>> ToQueryable(IQueryable<TEntity> query, IServiceProvider serviceProvider);
    }
}
