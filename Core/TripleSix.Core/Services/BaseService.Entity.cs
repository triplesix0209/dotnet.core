using Microsoft.EntityFrameworkCore;
using TripleSix.Core.Entities.Interfaces;
using TripleSix.Core.Persistences.Interfaces;
using TripleSix.Core.Services.Interfaces;
using TripleSix.Core.Types;
using TripleSix.Core.Types.Interfaces;

namespace TripleSix.Core.Services
{
    /// <summary>
    /// Service cơ bản xử lý entity.
    /// </summary>
    /// <typeparam name="TEntity">Loại entity xử lý.</typeparam>
    public abstract class BaseService<TEntity> : BaseService,
        IService<TEntity>
        where TEntity : class, IEntity
    {
        /// <summary>
        /// Database context.
        /// </summary>
        public IDbDataContext DbContext { get; set; }

        /// <inheritdoc/>
        public virtual Task<bool> AnyAsync(IQueryable<TEntity>? query = null, CancellationToken cancellationToken = default)
        {
            if (query == null) query = DbContext.Set<TEntity>();
            return query.AnyAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public virtual Task<long> CountAsync(IQueryable<TEntity>? query = null, CancellationToken cancellationToken = default)
        {
            if (query == null) query = DbContext.Set<TEntity>();
            return query.LongCountAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public virtual Task<TEntity?> GetFirstOrDefaultAsync(IQueryable<TEntity> query, CancellationToken cancellationToken = default)
        {
            return query.FirstOrDefaultAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<TEntity> GetFirstAsync(IQueryable<TEntity> query, CancellationToken cancellationToken = default)
        {
            var data = await GetFirstOrDefaultAsync(query, cancellationToken);
            if (data == null)
                throw new NullReferenceException(typeof(TEntity).Name);

            return data;
        }

        /// <inheritdoc/>
        public virtual Task<List<TEntity>> GetListAsync(IQueryable<TEntity> query, CancellationToken cancellationToken = default)
        {
            return query.ToListAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task<IPaging<TEntity>> GetPageAsync(IQueryable<TEntity> query, int page = 1, int size = 10, CancellationToken cancellationToken = default)
        {
            if (page <= 0) throw new ArgumentOutOfRangeException(nameof(page), "must be greater than 0");
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size), "must be greater than 0");

            var total = await CountAsync(query, cancellationToken);
            var items = total == 0
                ? new List<TEntity>()
                : await query
                    .Skip((page - 1) * size)
                    .Take(size)
                    .ToListAsync(cancellationToken);

            return new Paging<TEntity>(items, total, page, size);
        }

        /// <inheritdoc/>
        public virtual async Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await DbContext.Set<TEntity>()
                .AddAsync(entity, cancellationToken);

            await DbContext.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task UpdateAsync(TEntity entity, Action<TEntity> updateMethod, CancellationToken cancellationToken = default)
        {
            updateMethod(entity);
            DbContext.Set<TEntity>().Update(entity);

            await DbContext.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            DbContext.Set<TEntity>().Remove(entity);

            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
