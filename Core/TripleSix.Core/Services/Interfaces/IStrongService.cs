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
        /// Hàm phát sinh entity.
        /// </summary>
        /// <param name="entity">Entity đính kèm trong quá trình phát sinh mã.</param>
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Mã số được phát sinh.</returns>
        Task<string?> GenerateCode(TEntity? entity = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// Khởi tạo entity.
        /// </summary>
        /// <param name="entity">Entity sử dụng để ghi nhận.</param>
        /// <param name="generateCode">Có phát sinh code hay không? Nếu entity đã có code thì sẽ không phát sinh.</param>
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Entity đã được tạo.</returns>
        Task<TEntity> Create(TEntity entity, bool generateCode, CancellationToken cancellationToken = default);

        /// <summary>
        /// Cập nhật entity.
        /// </summary>
        /// <param name="id">Id của entity sẽ được cập nhật.</param>
        /// <param name="includeDeleted">Có tính các mục đã bị đánh dấu xóa khi tiến hành tìm kiếm.</param>
        /// <param name="updateMethod">Hàm thực hiện các thay đổi của entity.</param>
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Task xử lý.</returns>
        Task Update(Guid id, bool includeDeleted, Action<TEntity> updateMethod, CancellationToken cancellationToken = default);

        /// <summary>
        /// Cập nhật entity với Mapper.
        /// </summary>
        /// <param name="id">Id của entity sẽ được cập nhật.</param>
        /// <param name="includeDeleted">Có tính các mục đã bị đánh dấu xóa khi tiến hành tìm kiếm.</param>
        /// <param name="input">Data DTO dùng để dối chiếu và cập nhật entity.</param>
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Task xử lý.</returns>
        Task UpdateWithMapper(Guid id, bool includeDeleted, IDataDto input, CancellationToken cancellationToken = default);

        /// <summary>
        /// Xóa bỏ entity.
        /// </summary>
        /// <param name="id">Id của entity sẽ xóa.</param>
        /// <param name="includeDeleted">Có tính các mục đã bị đánh dấu xóa khi tiến hành tìm kiếm.</param>
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Task xử lý.</returns>
        Task HardDelete(Guid id, bool includeDeleted, CancellationToken cancellationToken = default);

        /// <summary>
        /// Tạm xóa entity.
        /// </summary>
        /// <param name="entity">Entity sẽ bị đánh dấu xóa.</param>
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Task xử lý.</returns>
        Task SoftDelete(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Tạm xóa entity.
        /// </summary>
        /// <param name="id">Id của entity sẽ xóa.</param>
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Task xử lý.</returns>
        Task SoftDelete(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Khôi phục các entity bị tạm xóa.
        /// </summary>
        /// <param name="entity">Entity sẽ bị đánh dấu xóa.</param>
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Task xử lý.</returns>
        Task Restore(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Khôi phục các entity bị tạm xóa.
        /// </summary>
        /// <param name="id">Id của entity sẽ xóa.</param>
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Task xử lý.</returns>
        Task Restore(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Kiểm tra có bất kỳ entity.
        /// </summary>
        /// <param name="includeDeleted">Có tính các mục đã bị đánh dấu xóa khi tiến hành tìm kiếm.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param
        /// <returns><c>True</c> nếu có bất kỳ entity nào tồn tại, ngược lại là <c>False</c>.</returns>
        Task<bool> Any(bool includeDeleted, CancellationToken cancellationToken = default);

        /// <summary>
        /// Đếm số lượng tất cả các entity.
        /// </summary>
        /// <param name="includeDeleted">Có tính các mục đã bị đánh dấu xóa khi tiến hành tìm kiếm.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param
        /// <returns>Số lượng entity.</returns>
        Task<long> Count(bool includeDeleted, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lấy entity theo Id và convert với Mapper, không có sẽ trả về null.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="id">Id tìm kiếm.</param>
        /// <param name="includeDeleted">Có tính các mục đã bị đánh dấu xóa khi tiến hành tìm kiếm.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param>
        /// <returns>Dữ liệu của entity khớp với id chỉ định, trả về null nếu không tìm thấy.</returns>
        Task<TResult?> GetOrDefaultById<TResult>(Guid id, bool includeDeleted, CancellationToken cancellationToken = default)
            where TResult : class;

        /// <summary>
        /// Lấy entity theo Id, không có sẽ trả về null.
        /// </summary>
        /// <param name="id">Id tìm kiếm.</param>
        /// <param name="includeDeleted">Có tính các mục đã bị đánh dấu xóa khi tiến hành tìm kiếm.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param>
        /// <returns>Entity khớp với id chỉ định, trả về null nếu không tìm thấy.</returns>
        Task<TEntity?> GetOrDefaultById(Guid id, bool includeDeleted, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lấy entity theo Id và convert với Mapper.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="id">Id tìm kiếm.</param>
        /// <param name="includeDeleted">Có tính các mục đã bị đánh dấu xóa khi tiến hành tìm kiếm.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param>
        /// <returns>Dữ liệu entity khớp với id chỉ định, nếu không tìm thấy sẽ trả lỗi.</returns>
        Task<TResult> GetById<TResult>(Guid id, bool includeDeleted, CancellationToken cancellationToken = default)
            where TResult : class;

        /// <summary>
        /// Lấy entity theo Id.
        /// </summary>
        /// <param name="id">Id tìm kiếm.</param>
        /// <param name="includeDeleted">Có tính các mục đã bị đánh dấu xóa khi tiến hành tìm kiếm.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param>
        /// <returns>Entity khớp với id chỉ định, nếu không tìm thấy sẽ trả lỗi.</returns>
        Task<TEntity> GetById(Guid id, bool includeDeleted, CancellationToken cancellationToken = default);

    }
}
