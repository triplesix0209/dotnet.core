using TripleSix.Core.Entities;
using TripleSix.Core.Types;

namespace TripleSix.Core.Services
{
    /// <summary>
    /// Service lấy dữ liệu.
    /// </summary>
    /// <typeparam name="TEntity">Loại entity xử lý.</typeparam>
    /// <typeparam name="TFilter">Loại dto làm bộ lọc.</typeparam>
    public interface IStrongServiceRead<TEntity, TFilter> : IStrongService<TEntity>
        where TEntity : class, IStrongEntity
        where TFilter : class, IEntityQueryableDto<TEntity>
    {
        /// <summary>
        /// Lấy phân trang các dữ liệu.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="input">Dữ liệu để tạo câu query.</param>
        /// <param name="page">Số trang.</param>
        /// <param name="size">Kích thước trang.</param>
        /// <returns>Dữ liệu phân trang của entity.</returns>
        Task<IPaging<TResult>> GetPage<TResult>(TFilter input, int page, int size)
            where TResult : class
            => GetPageByQueryModel<TResult>(input, page, size);
    }
}
