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
        /// <returns>Entity đã được tạo.</returns>
        Task<TEntity> Create(TEntity entity);

        /// <summary>
        /// Khởi tạo entity.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="entity">Entity sử dụng để ghi nhận.</param>
        /// <returns>Dữ liệu được map từ entity dã tạo.</returns>
        Task<TResult> Create<TResult>(TEntity entity)
            where TResult : class;

        /// <summary>
        /// Khởi tạo entity với Mapper.
        /// </summary>
        /// <param name="input">Dữ liệu đầu vào dùng để map sang entity.</param>
        /// <param name="afterMap">Hảm xử lý sau khi map dữ liệu, trước khi ghi nhận database.</param>
        /// <returns>Dữ liệu được map từ entity dã tạo.</returns>
        Task<TEntity> CreateWithMapper(IDto input, Action<TEntity>? afterMap = null);

        /// <summary>
        /// Khởi tạo entity với Mapper.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="input">Dữ liệu đầu vào dùng để map sang entity.</param>
        /// <param name="afterMap">Hảm xử lý sau khi map dữ liệu, trước khi ghi nhận database.</param>
        /// <returns>Dữ liệu được map từ entity dã tạo.</returns>
        Task<TResult> CreateWithMapper<TResult>(IDto input, Action<TEntity>? afterMap = null)
            where TResult : class;

        /// <summary>
        /// Cập nhật entity.
        /// </summary>
        /// <param name="entity">Entity sử dụng để cập nhận.</param>
        /// <returns>Entity sau chỉnh sửa.</returns>
        Task<TEntity> Update(TEntity entity);

        /// <summary>
        /// Cập nhật entity.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="entity">Entity sử dụng để cập nhận.</param>
        /// <returns>Dữ liệu được map từ entity sau chỉnh sửa.</returns>
        Task<TResult> Update<TResult>(TEntity entity)
            where TResult : class;

        /// <summary>
        /// Cập nhật entity với Mapper.
        /// </summary>
        /// <param name="entity">Entity sử dụng để cập nhận.</param>
        /// <param name="input">Data DTO dùng để dối chiếu và cập nhật entity.</param>
        /// <param name="afterMap">Hảm xử lý sau khi map dữ liệu, trước khi ghi nhận database.</param>
        /// <returns>Entity sau chỉnh sửa.</returns>
        Task<TEntity> UpdateWithMapper(TEntity entity, IDto input, Action<TEntity>? afterMap = null);

        /// <summary>
        /// Cập nhật entity với Mapper.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="entity">Entity sử dụng để cập nhận.</param>
        /// <param name="input">Data DTO dùng để dối chiếu và cập nhật entity.</param>
        /// <param name="afterMap">Hảm xử lý sau khi map dữ liệu, trước khi ghi nhận database.</param>
        /// <returns>Dữ liệu được map từ entity sau chỉnh sửa.</returns>
        Task<TResult> UpdateWithMapper<TResult>(TEntity entity, IDto input, Action<TEntity>? afterMap = null)
            where TResult : class;

        /// <summary>
        /// Xóa bỏ entity.
        /// </summary>
        /// <param name="entity">Entity sẽ bị xóa.</param>
        /// <returns>Task xử lý.</returns>
        Task HardDelete(TEntity entity);

        /// <summary>
        /// Kiểm tra có bất kỳ entity.
        /// </summary>
        /// <param name="query">Query sử dụng để truy vấn.</param>
        /// <returns><c>True</c> nếu có bất kỳ entity nào tồn tại, ngược lại là <c>False</c>.</returns>
        Task<bool> Any(IQueryable<TEntity>? query = default);

        /// <summary>
        /// Kiểm tra có bất kỳ entity.
        /// </summary>
        /// <param name="model">Model để tạo câu query.</param>
        /// <returns><c>True</c> nếu có bất kỳ entity nào tồn tại, ngược lại là <c>False</c>.</returns>
        Task<bool> AnyByQueryModel(IEntityQueryableDto<TEntity> model);

        /// <summary>
        /// Đếm số lượng tất cả các entity.
        /// </summary>
        /// <param name="query">Query sử dụng để truy vấn.</param>
        /// <returns>Số lượng entity.</returns>
        Task<long> Count(IQueryable<TEntity>? query = default);

        /// <summary>
        /// Đếm số lượng tất cả các entity.
        /// </summary>
        /// <param name="model">Model để tạo câu query.</param>
        /// <returns>Số lượng entity.</returns>
        Task<long> CountByQueryModel(IEntityQueryableDto<TEntity> model);

        /// <summary>
        /// Lấy entity đầu tiên và convert với Mapper, không có sẽ trả về null.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="query">Query sử dụng để truy vấn.</param>
        /// <returns>Dữ liệu của entity đầu tiên thỏa query, trả về null nếu không tìm thấy.</returns>
        Task<TResult?> GetFirstOrDefault<TResult>(IQueryable<TEntity>? query = default)
            where TResult : class;

        /// <summary>
        /// Lấy entity đầu tiên, không có sẽ trả về null.
        /// </summary>
        /// <param name="query">Query sử dụng để truy vấn.</param>
        /// <returns>Entity đầu tiên thỏa query, trả về null nếu không tìm thấy.</returns>
        Task<TEntity?> GetFirstOrDefault(IQueryable<TEntity>? query = default);

        /// <summary>
        /// Lấy entity đầu tiên và convert với Mapper, không có sẽ trả về null.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="model">Model để tạo câu query.</param>
        /// <returns>Dữ liệu của entity đầu tiên thỏa query, trả về null nếu không tìm thấy.</returns>
        Task<TResult?> GetFirstOrDefaultByQueryModel<TResult>(IEntityQueryableDto<TEntity> model)
            where TResult : class;

        /// <summary>
        /// Lấy entity đầu tiên, không có sẽ trả về null.
        /// </summary>
        /// <param name="model">Model để tạo câu query.</param>
        /// <returns>Entity đầu tiên thỏa query, trả về null nếu không tìm thấy.</returns>
        Task<TEntity?> GetFirstOrDefaultByQueryModel(IEntityQueryableDto<TEntity> model);

        /// <summary>
        /// Lấy entity đầu tiên và convert với Mapper.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="query">Query sử dụng để truy vấn.</param>
        /// <returns>Dữ liệu của entity đầu tiên thỏa query, nếu không tìm thấy sẽ trả lỗi.</returns>
        Task<TResult> GetFirst<TResult>(IQueryable<TEntity>? query = default)
            where TResult : class;

        /// <summary>
        /// Lấy entity đầu tiên.
        /// </summary>
        /// <param name="query">Query sử dụng để truy vấn.</param>
        /// <returns>Entity đầu tiên thỏa query, nếu không tìm thấy sẽ trả lỗi.</returns>
        Task<TEntity> GetFirst(IQueryable<TEntity>? query = default);

        /// <summary>
        /// Lấy entity đầu tiên và convert với Mapper.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="model">Model để tạo câu query.</param>
        /// <returns>Dữ liệu của entity đầu tiên thỏa query, nếu không tìm thấy sẽ trả lỗi.</returns>
        Task<TResult> GetFirstByQueryModel<TResult>(IEntityQueryableDto<TEntity> model)
            where TResult : class;

        /// <summary>
        /// Lấy entity đầu tiên.
        /// </summary>
        /// <param name="model">Model để tạo câu query.</param>
        /// <returns>Entity đầu tiên thỏa query, nếu không tìm thấy sẽ trả lỗi.</returns>
        Task<TEntity> GetFirstByQueryModel(IEntityQueryableDto<TEntity> model);

        /// <summary>
        /// Lấy danh sách các entity và convert với Mapper.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="query">Query sử dụng để truy vấn, mặc định sẽ query toàn bộ bảng.</param>
        /// <returns>Danh sách dữ liệu của entity.</returns>
        Task<List<TResult>> GetList<TResult>(IQueryable<TEntity>? query = default)
            where TResult : class;

        /// <summary>
        /// Lấy danh sách các entity.
        /// </summary>
        /// <param name="query">Query sử dụng để truy vấn, mặc định sẽ query toàn bộ bảng.</param>
        /// <returns>Danh sách entity.</returns>
        Task<List<TEntity>> GetList(IQueryable<TEntity>? query = default);

        /// <summary>
        /// Lấy danh sách các entity và convert với Mapper.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="model">Model để tạo câu query.</param>
        /// <returns>Danh sách dữ liệu của entity.</returns>
        Task<List<TResult>> GetListByQueryModel<TResult>(IEntityQueryableDto<TEntity> model)
            where TResult : class;

        /// <summary>
        /// Lấy danh sách các entity.
        /// </summary>
        /// <param name="model">Model để tạo câu query.</param>
        /// <returns>Danh sách entity.</returns>
        Task<List<TEntity>> GetListByQueryModel(IEntityQueryableDto<TEntity> model);

        /// <summary>
        /// Lấy phân trang các entity và convert với Mapper.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="query">Query sử dụng để truy vấn.</param>
        /// <param name="page">Số trang.</param>
        /// <param name="size">Kích thước trang.</param>
        /// <returns>Dữ liệu phân trang của entity.</returns>
        Task<IPaging<TResult>> GetPage<TResult>(IQueryable<TEntity>? query = default, int page = 1, int size = 10)
            where TResult : class;

        /// <summary>
        /// Lấy phân trang các entity.
        /// </summary>
        /// <param name="query">Query sử dụng để truy vấn.</param>
        /// <param name="page">Số trang.</param>
        /// <param name="size">Kích thước trang.</param>
        /// <returns>Dữ liệu phân trang entity.</returns>
        Task<IPaging<TEntity>> GetPage(IQueryable<TEntity>? query = default, int page = 1, int size = 10);

        /// <summary>
        /// Lấy phân trang các entity và convert với Mapper.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="model">Model để tạo câu query.</param>
        /// <param name="page">Số trang.</param>
        /// <param name="size">Kích thước trang.</param>
        /// <returns>Dữ liệu phân trang của entity.</returns>
        Task<IPaging<TResult>> GetPageByQueryModel<TResult>(IEntityQueryableDto<TEntity> model, int page = 1, int size = 10)
            where TResult : class;

        /// <summary>
        /// Lấy phân trang các entity.
        /// </summary>
        /// <param name="model">Model để tạo câu query.</param>
        /// <param name="page">Số trang.</param>
        /// <param name="size">Kích thước trang.</param>
        /// <returns>Dữ liệu phân trang entity.</returns>
        Task<IPaging<TEntity>> GetPageByQueryModel(IEntityQueryableDto<TEntity> model, int page = 1, int size = 10);
    }
}
