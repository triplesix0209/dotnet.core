using TripleSix.Core.Entities.Interfaces;
using TripleSix.Core.Types.Interfaces;

namespace TripleSix.Core.Services.Interfaces
{
    /// <summary>
    /// Service cơ bản xử lý entity.
    /// </summary>
    /// <typeparam name="TEntity">Loại entity xử lý.</typeparam>
    public interface IService<TEntity> : IService
        where TEntity : class, IEntity
    {
        /// <summary>
        /// Kiểm tra có bất kỳ entity thỏa query.
        /// </summary>
        /// <param name="query">Query sử dụng để truy vấn.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param
        /// <returns><c>True</c> nếu có bất kỳ entity nào tồn tại, ngược lại là <c>False</c>.</returns>
        Task<bool> AnyAsync(IQueryable<TEntity>? query = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Đếm số lượng tất cả các entity thỏa query.
        /// </summary>
        /// <param name="query">Query sử dụng để truy vấn.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param
        /// <returns>Số lượng entity.</returns>
        Task<long> CountAsync(IQueryable<TEntity>? query = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lấy entity đầu tiên thỏa query, hoặc không.
        /// </summary>
        /// <param name="query">Query sử dụng để truy vấn.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param>
        /// <returns>Entity đầu tiên thỏa query, trả về null nếu không tìm thấy.</returns>
        Task<TEntity?> GetFirstOrDefaultAsync(IQueryable<TEntity> query, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lấy entity đầu tiên thỏa query.
        /// </summary>
        /// <param name="query">Query sử dụng để truy vấn.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param>
        /// <returns>Entity đầu tiên thỏa query, nếu không tìm thấy sẽ trả lỗi.</returns>
        Task<TEntity> GetFirstAsync(IQueryable<TEntity> query, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lấy danh sách các entity thỏa query.
        /// </summary>
        /// <param name="query">Query sử dụng để truy vấn.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param>
        /// <returns>Danh sách entity.</returns>
        Task<List<TEntity>> GetListAsync(IQueryable<TEntity> query, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lấy phân trang các entity thỏa query.
        /// </summary>
        /// <param name="query">Query sử dụng để truy vấn.</param>
        /// <param name="page">Số trang.</param>
        /// <param name="size">Kích thước trang.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param>
        /// <returns>Dữ liệu phân trang entity.</returns>
        Task<IPaging<TEntity>> GetPageAsync(IQueryable<TEntity> query, int page = 1, int size = 10, CancellationToken cancellationToken = default);

        /// <summary>
        /// Khởi tạo entity.
        /// </summary>
        /// <param name="entity">Entity sử dụng để ghi nhận.</param>
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Task xử lý.</returns>
        Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Cập nhật entity.
        /// </summary>
        /// <param name="entity">Entity sử dụng để cập nhận.</param>
        /// <param name="updateMethod">Hàm thực hiện các thay đổi của entity.</param>
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Task xử lý.</returns>
        Task UpdateAsync(TEntity entity, Action<TEntity> updateMethod, CancellationToken cancellationToken = default);

        /// <summary>
        /// Xóa vĩnh viễn entity.
        /// </summary>
        /// <param name="entity">Entity sẽ bị xóa.</param>
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Task xử lý.</returns>
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    }
}
