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
        /// Lấy entity đầu tiên và convert với Mapper theo Id, không có sẽ trả về null.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="id">Id đối tượng.</param>
        /// <returns>Dữ liệu của entity đầu tiên thỏa query, trả về null nếu không tìm thấy.</returns>
        Task<TResult?> GetFirstOrDefaultById<TResult>(Guid id)
            where TResult : class;

        /// <summary>
        /// Lấy entity đầu tiên theo Id, không có sẽ trả về null.
        /// </summary>
        /// <param name="id">Id đối tượng.</param>
        /// <returns>Entity đầu tiên thỏa query, trả về null nếu không tìm thấy.</returns>
        Task<TEntity?> GetFirstOrDefaultById(Guid id);

        /// <summary>
        /// Lấy entity đầu tiên và convert với Mapper theo Id.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="id">Id đối tượng.</param>
        /// <returns>Dữ liệu của entity đầu tiên thỏa query, nếu không tìm thấy sẽ trả lỗi.</returns>
        Task<TResult> GetFirstById<TResult>(Guid id)
            where TResult : class;

        /// <summary>
        /// Lấy entity đầu tiên theo Id.
        /// </summary>
        /// <param name="id">Id đối tượng.</param>
        /// <returns>Entity đầu tiên thỏa query, nếu không tìm thấy sẽ trả lỗi.</returns>
        Task<TEntity> GetFirstById(Guid id);

        /// <summary>
        /// Tạm xóa entity.
        /// </summary>
        /// <param name="entity">Entity sẽ bị đánh dấu xóa.</param>
        /// <returns>Task xử lý.</returns>
        Task SoftDelete(TEntity entity);

        /// <summary>
        /// Khôi phục các entity bị tạm xóa.
        /// </summary>
        /// <param name="entity">Entity sẽ bị đánh dấu xóa.</param>
        /// <returns>Task xử lý.</returns>
        Task Restore(TEntity entity);
    }
}
