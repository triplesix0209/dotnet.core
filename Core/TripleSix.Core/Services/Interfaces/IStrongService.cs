using TripleSix.Core.Entities.Interfaces;

namespace TripleSix.Core.Services.Interfaces
{
    /// <summary>
    /// Service xử lý strong entity.
    /// </summary>
    /// <typeparam name="TEntity">Loại entity xử lý.</typeparam>
    public interface IStrongService<TEntity> : IService<TEntity>
        where TEntity : class, IStrongEntity
    {
        /// <summary>
        /// Hàm phát sinh entity.
        /// </summary>
        /// <param name="entity">Entity sử dụng.</param>
        /// <returns>Mã số được phát sinh.</returns>
        Task<string?> GenerateCode(TEntity entity);

        /// <summary>
        /// Khởi tạo entity.
        /// </summary>
        /// <param name="entity">Entity sử dụng để ghi nhận.</param>
        /// <param name="generateCode">Có phát sinh code hay không? Nếu entity đã có code thì sẽ không phát sinh.</param>
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Task xử lý.</returns>
        Task CreateAsync(TEntity entity, bool generateCode, CancellationToken cancellationToken = default);

        /// <summary>
        /// Đánh dấu xóa entity.
        /// </summary>
        /// <param name="entity">Entity sẽ bị đánh dấu xóa.</param>
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Task xử lý.</returns>
        Task SoftDeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Khôi phục các entity bị soft delete.
        /// </summary>
        /// <param name="entity">Entity sẽ bị đánh dấu xóa.</param>
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Task xử lý.</returns>
        Task RestoreAsync(TEntity entity, CancellationToken cancellationToken = default);
    }
}
