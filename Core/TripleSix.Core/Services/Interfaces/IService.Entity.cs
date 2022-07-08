using TripleSix.Core.Entities.Interfaces;

namespace TripleSix.Core.Services.Interfaces
{
    /// <summary>
    /// Service cơ bản xử lý entity.
    /// </summary>
    /// <typeparam name="TEntity">Loại entity xử lý.</typeparam>
    public interface IService<TEntity>
        where TEntity : IEntity
    {
        /// <summary>
        /// Khởi tạo mục.
        /// </summary>
        /// <param name="entity">Entity sử dụng để ghi nhận.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param>
        /// <returns>Entity sau khi đã ghi nhận.</returns>
        Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lấy danh sách các mục.
        /// </summary>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param>
        /// <returns>Danh sách entity.</returns>
        Task<IList<TEntity>> GetList(CancellationToken cancellationToken = default);
    }
}
