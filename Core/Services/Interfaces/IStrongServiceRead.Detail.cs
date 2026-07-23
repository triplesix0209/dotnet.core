using TripleSix.Core.Entities;
using TripleSix.Core.Types;

namespace TripleSix.Core.Services
{
    /// <summary>
    /// Service lấy dữ liệu.
    /// </summary>
    /// <typeparam name="TEntity">Loại entity xử lý.</typeparam>
    /// <typeparam name="TListFilter">Loại dto làm bộ lọc danh sách.</typeparam>
    /// <typeparam name="TDetailFilter">Loại dto làm bộ lọc chi tiết.</typeparam>
    public interface IStrongServiceRead<TEntity, TListFilter, TDetailFilter> : IStrongService<TEntity>
        where TEntity : class, IStrongEntity
        where TListFilter : class, IEntityQueryableDto<TEntity>
        where TDetailFilter : class, IEntityQueryableDto<TEntity>
    {
        /// <summary>
        /// Lấy phân trang các dữ liệu.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="input">Dữ liệu để tạo câu query.</param>
        /// <param name="page">Số trang.</param>
        /// <param name="size">Kích thước trang.</param>
        /// <returns>Dữ liệu phân trang của entity.</returns>
        Task<IPaging<TResult>> GetPage<TResult>(TListFilter input, int page, int size)
            where TResult : class
            => GetPageByQueryModel<TResult>(input, page, size);

        /// <summary>
        /// Lấy danh sách các dữ liệu.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="input">Dữ liệu để tạo câu query.</param>
        /// <returns>Dữ liệu phân trang của entity.</returns>
        Task<List<TResult>> GetList<TResult>(TListFilter input)
            where TResult : class
            => GetListByQueryModel<TResult>(input);

        /// <summary>
        /// Lấy mục dữ liệu đầu tiên.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="input">Dữ liệu để tạo câu query.</param>
        /// <returns>Dữ liệu phân trang của entity.</returns>
        Task<TResult> GetFirst<TResult>(TDetailFilter input)
            where TResult : class
            => GetFirstByQueryModel<TResult>(input);

        /// <summary>
        /// Lấy mục dữ liệu đầu tiên, không có sẽ trả về null.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="input">Dữ liệu để tạo câu query.</param>
        /// <returns>Dữ liệu phân trang của entity.</returns>
        Task<TResult?> GetFirstOrDefault<TResult>(TDetailFilter input)
            where TResult : class
            => GetFirstOrDefaultByQueryModel<TResult>(input);
    }
}
