using TripleSix.Core.Entities;
using TripleSix.Core.Types;

namespace TripleSix.Core.Services
{
    /// <summary>
    /// Service cơ bản xử lý entity.
    /// </summary>
    /// <typeparam name="TEntity">Loại entity xử lý.</typeparam>
    public interface IService<TEntity> : IService
        where TEntity : class, IEntity
    {
        /// <summary>
        /// Khởi tạo entity.
        /// </summary>
        /// <param name="entity">Entity sử dụng để ghi nhận.</param>
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Entity đã được tạo.</returns>
        Task<TEntity> Create(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Khởi tạo entity với Mapper.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="input">Dữ liệu đầu vào dùng để map sang entity.</param>
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Dữ liệu được map từ entity dã tạo.</returns>
        Task<TResult> CreateWithMapper<TResult>(IDto input, CancellationToken cancellationToken = default)
            where TResult : class;

        /// <summary>
        /// Cập nhật entity.
        /// </summary>
        /// <param name="entity">Entity sử dụng để cập nhận.</param>
        /// <param name="updateMethod">Hàm thực hiện các thay đổi của entity.</param>
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Task xử lý.</returns>
        Task Update(TEntity entity, Action<TEntity> updateMethod, CancellationToken cancellationToken = default);

        /// <summary>
        /// Cập nhật entity với Mapper.
        /// </summary>
        /// <param name="entity">Entity sử dụng để cập nhận.</param>
        /// <param name="input">Data DTO dùng để dối chiếu và cập nhật entity.</param>
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Task xử lý.</returns>
        Task UpdateWithMapper(TEntity entity, IDto input, CancellationToken cancellationToken = default);

        /// <summary>
        /// Xóa bỏ entity.
        /// </summary>
        /// <param name="entity">Entity sẽ bị xóa.</param>
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Task xử lý.</returns>
        Task HardDelete(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Kiểm tra có bất kỳ entity.
        /// </summary>
        /// <param name="query">Query sử dụng để truy vấn, mặc định sẽ query toàn bộ bảng.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param
        /// <returns><c>True</c> nếu có bất kỳ entity nào tồn tại, ngược lại là <c>False</c>.</returns>
        Task<bool> Any(IQueryable<TEntity>? query = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// Kiểm tra có bất kỳ entity.
        /// </summary>
        /// <param name="model">Model để tạo câu query.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param
        /// <returns><c>True</c> nếu có bất kỳ entity nào tồn tại, ngược lại là <c>False</c>.</returns>
        Task<bool> AnyByQueryModel(IQueryableDto<TEntity> model, CancellationToken cancellationToken = default);

        /// <summary>
        /// Đếm số lượng tất cả các entity.
        /// </summary>
        /// <param name="query">Query sử dụng để truy vấn, mặc định sẽ query toàn bộ bảng.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param
        /// <returns>Số lượng entity.</returns>
        Task<long> Count(IQueryable<TEntity>? query = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// Đếm số lượng tất cả các entity.
        /// </summary>
        /// <param name="model">Model để tạo câu query.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param
        /// <returns>Số lượng entity.</returns>
        Task<long> CountByQueryModel(IQueryableDto<TEntity> model, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lấy entity đầu tiên và convert với Mapper, không có sẽ trả về null.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="query">Query sử dụng để truy vấn.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param>
        /// <returns>Dữ liệu của entity đầu tiên thỏa query, trả về null nếu không tìm thấy.</returns>
        Task<TResult?> GetFirstOrDefault<TResult>(IQueryable<TEntity>? query = default, CancellationToken cancellationToken = default)
            where TResult : class;

        /// <summary>
        /// Lấy entity đầu tiên, không có sẽ trả về null.
        /// </summary>
        /// <param name="query">Query sử dụng để truy vấn.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param>
        /// <returns>Entity đầu tiên thỏa query, trả về null nếu không tìm thấy.</returns>
        Task<TEntity?> GetFirstOrDefault(IQueryable<TEntity>? query = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lấy entity đầu tiên và convert với Mapper, không có sẽ trả về null.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="model">Model để tạo câu query.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param>
        /// <returns>Dữ liệu của entity đầu tiên thỏa query, trả về null nếu không tìm thấy.</returns>
        Task<TResult?> GetFirstOrDefaultByQueryModel<TResult>(IQueryableDto<TEntity> model, CancellationToken cancellationToken = default)
            where TResult : class;

        /// <summary>
        /// Lấy entity đầu tiên, không có sẽ trả về null.
        /// </summary>
        /// <param name="model">Model để tạo câu query.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param>
        /// <returns>Entity đầu tiên thỏa query, trả về null nếu không tìm thấy.</returns>
        Task<TEntity?> GetFirstOrDefaultByQueryModel(IQueryableDto<TEntity> model, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lấy entity đầu tiên và convert với Mapper.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="query">Query sử dụng để truy vấn.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param>
        /// <returns>Dữ liệu của entity đầu tiên thỏa query, nếu không tìm thấy sẽ trả lỗi.</returns>
        Task<TResult> GetFirst<TResult>(IQueryable<TEntity>? query = default, CancellationToken cancellationToken = default)
            where TResult : class;

        /// <summary>
        /// Lấy entity đầu tiên.
        /// </summary>
        /// <param name="query">Query sử dụng để truy vấn.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param>
        /// <returns>Entity đầu tiên thỏa query, nếu không tìm thấy sẽ trả lỗi.</returns>
        Task<TEntity> GetFirst(IQueryable<TEntity>? query = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lấy entity đầu tiên và convert với Mapper.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="model">Model để tạo câu query.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param>
        /// <returns>Dữ liệu của entity đầu tiên thỏa query, nếu không tìm thấy sẽ trả lỗi.</returns>
        Task<TResult> GetFirstByQueryModel<TResult>(IQueryableDto<TEntity> model, CancellationToken cancellationToken = default)
            where TResult : class;

        /// <summary>
        /// Lấy entity đầu tiên.
        /// </summary>
        /// <param name="model">Model để tạo câu query.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param>
        /// <returns>Entity đầu tiên thỏa query, nếu không tìm thấy sẽ trả lỗi.</returns>
        Task<TEntity> GetFirstByQueryModel(IQueryableDto<TEntity> model, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lấy danh sách các entity và convert với Mapper.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="query">Query sử dụng để truy vấn, mặc định sẽ query toàn bộ bảng.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param>
        /// <returns>Danh sách dữ liệu của entity.</returns>
        Task<List<TResult>> GetList<TResult>(IQueryable<TEntity>? query = default, CancellationToken cancellationToken = default)
            where TResult : class;

        /// <summary>
        /// Lấy danh sách các entity.
        /// </summary>
        /// <param name="query">Query sử dụng để truy vấn, mặc định sẽ query toàn bộ bảng.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param>
        /// <returns>Danh sách entity.</returns>
        Task<List<TEntity>> GetList(IQueryable<TEntity>? query = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lấy danh sách các entity và convert với Mapper.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="model">Model để tạo câu query.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param>
        /// <returns>Danh sách dữ liệu của entity.</returns>
        Task<List<TResult>> GetListByQueryModel<TResult>(IQueryableDto<TEntity> model, CancellationToken cancellationToken = default)
            where TResult : class;

        /// <summary>
        /// Lấy danh sách các entity.
        /// </summary>
        /// <param name="model">Model để tạo câu query.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param>
        /// <returns>Danh sách entity.</returns>
        Task<List<TEntity>> GetListByQueryModel(IQueryableDto<TEntity> model, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lấy phân trang các entity và convert với Mapper.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="query">Query sử dụng để truy vấn.</param>
        /// <param name="page">Số trang.</param>
        /// <param name="size">Kích thước trang.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param>
        /// <returns>Dữ liệu phân trang của entity.</returns>
        Task<IPaging<TResult>> GetPage<TResult>(IQueryable<TEntity>? query = default, int page = 1, int size = 10, CancellationToken cancellationToken = default)
            where TResult : class;

        /// <summary>
        /// Lấy phân trang các entity.
        /// </summary>
        /// <param name="query">Query sử dụng để truy vấn.</param>
        /// <param name="page">Số trang.</param>
        /// <param name="size">Kích thước trang.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param>
        /// <returns>Dữ liệu phân trang entity.</returns>
        Task<IPaging<TEntity>> GetPage(IQueryable<TEntity>? query = default, int page = 1, int size = 10, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lấy phân trang các entity và convert với Mapper.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="model">Model để tạo câu query.</param>
        /// <param name="page">Số trang.</param>
        /// <param name="size">Kích thước trang.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param>
        /// <returns>Dữ liệu phân trang của entity.</returns>
        Task<IPaging<TResult>> GetPageByQueryModel<TResult>(IQueryableDto<TEntity> model, int page = 1, int size = 10, CancellationToken cancellationToken = default)
            where TResult : class;

        /// <summary>
        /// Lấy phân trang các entity.
        /// </summary>
        /// <param name="model">Model để tạo câu query.</param>
        /// <param name="page">Số trang.</param>
        /// <param name="size">Kích thước trang.</param>
        /// <param name="cancellationToken">Token để cancel tiến trình.</param>
        /// <returns>Dữ liệu phân trang entity.</returns>
        Task<IPaging<TEntity>> GetPageByQueryModel(IQueryableDto<TEntity> model, int page = 1, int size = 10, CancellationToken cancellationToken = default);
    }
}
