using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TripleSix.Core.Entities;

namespace TripleSix.Core.Types
{
    /// <summary>
    /// Model cho phép chuyển thành <see cref="IQueryable"/>.
    /// </summary>
    /// <typeparam name="TEntity">Loại entity.</typeparam>
    public interface IEntityQueryableDto<TEntity>
        where TEntity : IEntity
    {
        /// <summary>
        /// Chuyển hóa thành <see cref="IQueryable"/>.
        /// </summary>
        /// <param name="query">Câu query gốc.</param>
        /// <param name="configuration">Configuration.</param>
        /// <param name="httpContextAccessor">Http context accessor.</param>
        /// <returns><see cref="IQueryable"/>.</returns>
        IQueryable<TEntity> ToQueryable(IQueryable<TEntity> query, IConfiguration configuration, IHttpContextAccessor? httpContextAccessor);
    }
}
