using TripleSix.Core.Entities;
using TripleSix.Core.Types;

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
        /// Cập nhật entity.
        /// </summary>
        /// <param name="id">Id entity sử dụng để cập nhận.</param>
        /// <param name="updateAction">Hàm thực hiện thao tác chỉnh sửa.</param>
        /// <returns>Entity sau chỉnh sửa.</returns>
        Task<TEntity> Update(Guid id, Action<TEntity> updateAction);

        /// <summary>
        /// Cập nhật entity.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="id">Id entity sử dụng để cập nhận.</param>
        /// <param name="updateAction">Hàm thực hiện thao tác chỉnh sửa.</param>
        /// <returns>Dữ liệu được map từ entity sau chỉnh sửa.</returns>
        Task<TResult> Update<TResult>(Guid id, Action<TEntity> updateAction)
            where TResult : class;

        /// <summary>
        /// Cập nhật entity với Mapper.
        /// </summary>
        /// <param name="id">Id entity sử dụng để cập nhận.</param>
        /// <param name="input">Data DTO dùng để dối chiếu và cập nhật entity.</param>
        /// <param name="afterMap">Hảm xử lý sau khi map dữ liệu, trước khi ghi nhận database.</param>
        /// <returns>Entity sau được chỉnh sửa.</returns>
        Task<TEntity> UpdateWithMapper(Guid id, IDto input, Action<TEntity>? afterMap = null);

        /// <summary>
        /// Cập nhật entity với Mapper.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="id">Id entity sử dụng để cập nhận.</param>
        /// <param name="input">Data DTO dùng để dối chiếu và cập nhật entity.</param>
        /// <param name="afterMap">Hảm xử lý sau khi map dữ liệu, trước khi ghi nhận database.</param>
        /// <returns>Dữ liệu được map từ entity sau chỉnh sửa.</returns>
        Task<TResult> UpdateWithMapper<TResult>(Guid id, IDto input, Action<TEntity>? afterMap = null)
            where TResult : class;

        /// <summary>
        /// Xóa bỏ entity.
        /// </summary>
        /// <param name="id">Id entity sẽ bị xóa.</param>
        /// <returns>Task xử lý.</returns>
        Task HardDelete(Guid id);

        /// <summary>
        /// Tạm xóa entity.
        /// </summary>
        /// <param name="entity">Entity sẽ bị đánh dấu xóa.</param>
        /// <returns>Entity sau xử lý.</returns>
        Task<TEntity> SoftDelete(TEntity entity);

        /// <summary>
        /// Tạm xóa entity.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="entity">Entity sẽ bị đánh dấu xóa.</param>
        /// <returns>Dữ liệu được map từ entity sau xử lý.</returns>
        Task<TResult> SoftDelete<TResult>(TEntity entity)
            where TResult : class;

        /// <summary>
        /// Tạm xóa entity.
        /// </summary>
        /// <param name="id">Id entity sẽ bị đánh dấu xóa.</param>
        /// <returns>Entity sau xử lý.</returns>
        Task<TEntity> SoftDelete(Guid id);

        /// <summary>
        /// Tạm xóa entity.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="id">Id entity sẽ bị đánh dấu xóa.</param>
        /// <returns>Dữ liệu được map từ entity sau xử lý.</returns>
        Task<TResult> SoftDelete<TResult>(Guid id)
            where TResult : class;

        /// <summary>
        /// Khôi phục các entity bị tạm xóa.
        /// </summary>
        /// <param name="entity">Entity sẽ bị đánh dấu xóa.</param>
        /// <returns>Entity sau xử lý.</returns>
        Task<TEntity> Restore(TEntity entity);

        /// <summary>
        /// Khôi phục các entity bị tạm xóa.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="entity">Entity sẽ bị đánh dấu xóa.</param>
        /// <returns>Dữ liệu được map từ entity sau xử lý.</returns>
        Task<TResult> Restore<TResult>(TEntity entity)
            where TResult : class;

        /// <summary>
        /// Khôi phục các entity bị tạm xóa.
        /// </summary>
        /// <param name="id">Id entity sẽ bị đánh dấu xóa.</param>
        /// <returns>Entity sau xử lý.</returns>
        Task<TEntity> Restore(Guid id);

        /// <summary>
        /// Khôi phục các entity bị tạm xóa.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="id">Id entity sẽ bị đánh dấu xóa.</param>
        /// <returns>Dữ liệu được map từ entity sau xử lý.</returns>
        Task<TResult> Restore<TResult>(Guid id)
            where TResult : class;

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
    }
}
