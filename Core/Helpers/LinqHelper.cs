using System.Linq.Expressions;
using Autofac;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using TripleSix.Core.Entities;
using TripleSix.Core.Exceptions;

namespace TripleSix.Core.Helpers
{
    /// <summary>
    /// Helper xử lý LINQ.
    /// </summary>
    public static class LinqHelper
    {
        /// <summary>
        /// Thêm where lọc các mục không bị soft delete.
        /// </summary>
        /// <typeparam name="TEntity">Loại entity.</typeparam>
        /// <param name="query">Câu query cần xử lý.</param>
        /// <returns>Câu query đã được xử lý.</returns>
        public static IQueryable<TEntity> WhereNotDeleted<TEntity>(
            this IQueryable<TEntity> query)
            where TEntity : IStrongEntity
        {
            return query.Where(x => x.DeleteAt == null);
        }

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

        /// <summary>
        /// Lấy mục đầu tiên kèm Automapper, nếu không có sẽ báo lỗi <see cref="EntityNotFoundException"/>.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu sẽ chuyển đổi.</typeparam>
        /// <param name="query">Câu query cần xử lý.</param>
        /// <param name="mapper"><see cref="IMapper"/>.</param>
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Mục dữ liệu đầu tiên.</returns>
        public static async Task<TResult> FirstAsync<TResult>(this IQueryable<IEntity> query, IMapper mapper, CancellationToken cancellationToken = default)
            where TResult : class
        {
            var item = typeof(TResult).IsAssignableTo<IEntity>()
                ? await query.FirstOrDefaultAsync(cancellationToken) as TResult
                : await query.ProjectTo<TResult>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);

            if (item == null)
            {
                var entityType = query.GetType().GetInterfaces()
                    .First(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IQueryable<>))
                    .GetGenericArguments()[0];
                throw new EntityNotFoundException(entityType);
            }

            return item;
        }

        /// <summary>
        /// Lấy mục đầu tiên kèm Automapper, nếu không có sẽ trả về null.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu sẽ chuyển đổi.</typeparam>
        /// <param name="query">Câu query cần xử lý.</param>
        /// <param name="mapper"><see cref="IMapper"/>.</param>
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Mục dữ liệu đầu tiên.</returns>
        public static async Task<TResult?> FirstOrDefaultAsync<TResult>(this IQueryable<IEntity> query, IMapper mapper, CancellationToken cancellationToken = default)
            where TResult : class
        {
            if (typeof(TResult).IsAssignableTo<IEntity>())
                return await query.FirstOrDefaultAsync(cancellationToken) as TResult;

            return await query
                .ProjectTo<TResult>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Lấy mảng dữ liệu Automapper.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu sẽ chuyển đổi.</typeparam>
        /// <param name="query">Câu query cần xử lý.</param>
        /// <param name="mapper"><see cref="IMapper"/>.</param>
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Mảng dữ liệu.</returns>
        public static async Task<TResult[]> ToArrayAsync<TResult>(this IQueryable<IEntity> query, IMapper mapper, CancellationToken cancellationToken = default)
            where TResult : class
        {
            if (typeof(TResult).IsAssignableTo<IEntity>())
                return (await query.ToArrayAsync(cancellationToken)).Cast<TResult>().ToArray();

            return await query
                .ProjectTo<TResult>(mapper.ConfigurationProvider)
                .ToArrayAsync(cancellationToken);
        }

        /// <summary>
        /// Lấy danh sách dữ liệu Automapper.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu sẽ chuyển đổi.</typeparam>
        /// <param name="query">Câu query cần xử lý.</param>
        /// <param name="mapper"><see cref="IMapper"/>.</param>
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Danh sách dữ liệu.</returns>
        public static async Task<List<TResult>> ToListAsync<TResult>(this IQueryable<IEntity> query, IMapper mapper, CancellationToken cancellationToken = default)
            where TResult : class
        {
            if (typeof(TResult).IsAssignableTo<IEntity>())
                return (await query.ToArrayAsync(cancellationToken)).Cast<TResult>().ToList();

            return await query
                .ProjectTo<TResult>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}