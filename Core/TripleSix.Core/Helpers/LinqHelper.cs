using System.Linq.Expressions;

namespace TripleSix.Core.Helpers
{
    /// <summary>
    /// Helper xử lý LINQ.
    /// </summary>
    public static class LinqHelper
    {
        /// <summary>
        /// Thêm where cho câu query khi thỏa điều kiện.
        /// </summary>
        /// <typeparam name="TSource">Loại dữ liệu.</typeparam>
        /// <param name="query">Câu query cần xử lý.</param>
        /// <param name="condition">Điều kiện so sánh.</param>
        /// <param name="predicate">Nội dung câu where.</param>
        /// <returns>Câu query đã được xử lý.</returns>
        public static IQueryable<TSource> WhereIf<TSource>(
            this IQueryable<TSource> query,
            bool condition,
            Expression<Func<TSource, bool>> predicate)
        {
            return condition ? query.Where(predicate) : query;
        }
    }
}