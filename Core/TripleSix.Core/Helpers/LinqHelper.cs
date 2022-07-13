using System.Linq.Expressions;

namespace TripleSix.Core.Helpers
{
    /// <summary>
    /// Helper xử lý LINQ.
    /// </summary>
    public static class LinqHelper
    {
        /// <summary>
        /// Thêm where vào query khi thỏa điều kiện.
        /// </summary>
        /// <typeparam name="TEntity">Loại entity.</typeparam>
        /// <param name="query">Câu query cần xử lý.</param>
        /// <param name="condition">Điều kiện.</param>
        /// <param name="predicate">Nội dung câu where.</param>
        /// <returns>Câu query đã được xử lý.</returns>
        public static IQueryable<TEntity> WhereIf<TEntity>(
            this IQueryable<TEntity> query,
            bool condition,
            Expression<Func<TEntity, bool>> predicate)
        {
            return condition ? query.Where(predicate) : query;
        }

        /// <summary>
        /// Thêm where or vào query.
        /// </summary>
        /// <typeparam name="TEntity">Loại entity.</typeparam>
        /// <param name="query">Câu query cần xử lý.</param>
        /// <param name="predicates">Danh sách các điều kiện.</param>
        /// <returns>Câu query đã được xử lý.</returns>
        public static IQueryable<TEntity> WhereOrs<TEntity>(
            this IQueryable<TEntity> query,
            params Expression<Func<TEntity, bool>>[] predicates)
        {
            var expr = PredicateBuilder.New<TEntity>();
            foreach (var predicate in predicates)
                expr = expr.Or(predicate);
            return query.Where(expr);
        }
    }
}