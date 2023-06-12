using TripleSix.Core.Entities;

namespace TripleSix.Core.Services
{
    /// <summary>
    /// Service xử lý strong entity.
    /// </summary>
    /// <typeparam name="TEntity">Loại entity xử lý.</typeparam>
    public interface IStrongService<TEntity> : IService<TEntity>
        where TEntity : class, IStrongEntity
    {
        /// <summary>
        /// Tạm xóa entity.
        /// </summary>
        /// <param name="entity">Entity sẽ bị đánh dấu xóa.</param>
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Task xử lý.</returns>
        Task SoftDelete(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Khôi phục các entity bị tạm xóa.
        /// </summary>
        /// <param name="entity">Entity sẽ bị đánh dấu xóa.</param>
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Task xử lý.</returns>
        Task Restore(TEntity entity, CancellationToken cancellationToken = default);
    }
}
