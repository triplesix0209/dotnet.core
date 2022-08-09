#pragma warning disable SA1401 // Fields should be private

using Microsoft.EntityFrameworkCore;
using TripleSix.Core.Entities;
using TripleSix.Core.Exceptions;
using TripleSix.Core.Helpers;
using TripleSix.Core.Persistences;
using TripleSix.Core.Types;

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
        internal readonly IDbDataContext _db;

        /// <summary>
        /// Khởi tạo <see cref="BaseService{TEntity, TDbDataContext}"/>.
        /// </summary>
        /// <param name="db"><see cref="IDbDataContext"/>.</param>
        public BaseService(IDbDataContext db)
        {
            _db = db;
        }

        /// <inheritdoc/>
        public virtual async Task<TEntity> Create(TEntity entity, CancellationToken cancellationToken = default)
        {
            using var activity = StartTraceMethodActivity();

            await _db.Set<TEntity>().AddAsync(entity, cancellationToken);
            await _db.SaveChangesAsync(true, cancellationToken);
            return entity;
        }

        /// <inheritdoc/>
        public async Task<TResult> CreateWithMapper<TResult>(IDto input, CancellationToken cancellationToken = default)
            where TResult : class
        {
            var entity = Mapper.MapData<IDto, TEntity>(input);
            var result = await Create(entity, cancellationToken);
            return Mapper.MapData<TEntity, TResult>(result);
        }

        /// <inheritdoc/>
        public virtual async Task Update(TEntity entity, Action<TEntity> updateMethod, CancellationToken cancellationToken = default)
        {
            using var activity = StartTraceMethodActivity();

            updateMethod(entity);
            _db.Set<TEntity>().Update(entity);

            await _db.SaveChangesAsync(true, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task UpdateWithMapper(TEntity entity, IDto input, CancellationToken cancellationToken = default)
        {
            if (!input.IsAnyPropertyChanged()) return;

            await Update(
                entity,
                e => Mapper.MapUpdate(input, e),
                cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task HardDelete(TEntity entity, CancellationToken cancellationToken = default)
        {
            using var activity = StartTraceMethodActivity();

            _db.Set<TEntity>().Remove(entity);

            await _db.SaveChangesAsync(true, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<bool> Any(IQueryable<TEntity>? query = default, CancellationToken cancellationToken = default)
        {
            using var activity = StartTraceMethodActivity();

            if (query == null) query = _db.Set<TEntity>();

            return await query.AnyAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public Task<bool> AnyByModel(IQueryableModel<TEntity> model, CancellationToken cancellationToken = default)
        {
            return Any(model.ToQueryable(_db), cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<long> Count(IQueryable<TEntity>? query = default, CancellationToken cancellationToken = default)
        {
            using var activity = StartTraceMethodActivity();

            if (query == null) query = _db.Set<TEntity>();

            return await query.LongCountAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public Task<long> CountByModel(IQueryableModel<TEntity> model, CancellationToken cancellationToken = default)
        {
            return Count(model.ToQueryable(_db), cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<TResult?> GetFirstOrDefault<TResult>(IQueryable<TEntity>? query = default, CancellationToken cancellationToken = default)
            where TResult : class
        {
            using var activity = StartTraceMethodActivity();

            if (query == null) query = _db.Set<TEntity>();
            return await query.FirstOrDefaultAsync<TResult>(Mapper, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<TEntity?> GetFirstOrDefault(IQueryable<TEntity>? query = default, CancellationToken cancellationToken = default)
        {
            return GetFirstOrDefault<TEntity>(query, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<TResult?> GetFirstOrDefaultByModel<TResult>(IQueryableModel<TEntity> model, CancellationToken cancellationToken = default)
            where TResult : class
        {
            return GetFirstOrDefault<TResult>(model.ToQueryable(_db), cancellationToken);
        }

        /// <inheritdoc/>
        public Task<TEntity?> GetFirstOrDefaultByModel(IQueryableModel<TEntity> model, CancellationToken cancellationToken = default)
        {
            return GetFirstOrDefault(model.ToQueryable(_db), cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<TResult> GetFirst<TResult>(IQueryable<TEntity>? query = default, CancellationToken cancellationToken = default)
            where TResult : class
        {
            var data = await GetFirstOrDefault<TResult>(query, cancellationToken);
            if (data == null) throw new EntityNotFoundException(typeof(TEntity));

            return data;
        }

        /// <inheritdoc/>
        public Task<TEntity> GetFirst(IQueryable<TEntity>? query = default, CancellationToken cancellationToken = default)
        {
            return GetFirst<TEntity>(query, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<TResult> GetFirstByModel<TResult>(IQueryableModel<TEntity> model, CancellationToken cancellationToken = default)
            where TResult : class
        {
            return GetFirst<TResult>(model.ToQueryable(_db), cancellationToken);
        }

        /// <inheritdoc/>
        public Task<TEntity> GetFirstByModel(IQueryableModel<TEntity> model, CancellationToken cancellationToken = default)
        {
            return GetFirst(model.ToQueryable(_db), cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<List<TResult>> GetList<TResult>(IQueryable<TEntity>? query = default, CancellationToken cancellationToken = default)
            where TResult : class
        {
            using var activity = StartTraceMethodActivity();

            if (query == null) query = _db.Set<TEntity>();
            return await query.ToListAsync<TResult>(Mapper, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<List<TEntity>> GetList(IQueryable<TEntity>? query = default, CancellationToken cancellationToken = default)
        {
            return GetList<TEntity>(query, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<List<TResult>> GetListByModel<TResult>(IQueryableModel<TEntity> model, CancellationToken cancellationToken = default)
            where TResult : class
        {
            return GetList<TResult>(model.ToQueryable(_db), cancellationToken);
        }

        /// <inheritdoc/>
        public Task<List<TEntity>> GetListByModel(IQueryableModel<TEntity> model, CancellationToken cancellationToken = default)
        {
            return GetList(model.ToQueryable(_db), cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IPaging<TResult>> GetPage<TResult>(IQueryable<TEntity>? query = default, int page = 1, int size = 10, CancellationToken cancellationToken = default)
            where TResult : class
        {
            using var activity = StartTraceMethodActivity();

            if (page <= 0) throw new ArgumentOutOfRangeException(nameof(page), "must be greater than 0");
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size), "must be greater than 0");

            var result = new Paging<TResult>(page, size);
            if (query == null) query = _db.Set<TEntity>();

            result.Total = await query.LongCountAsync(cancellationToken);
            result.Items = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync<TResult>(Mapper, cancellationToken);
            return result;
        }

        /// <inheritdoc/>
        public Task<IPaging<TEntity>> GetPage(IQueryable<TEntity>? query = default, int page = 1, int size = 10, CancellationToken cancellationToken = default)
        {
            return GetPage<TEntity>(query, page, size, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<IPaging<TResult>> GetPageByModel<TResult>(IQueryableModel<TEntity> model, int page = 1, int size = 10, CancellationToken cancellationToken = default)
            where TResult : class
        {
            return GetPage<TResult>(model.ToQueryable(_db), page, size, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<IPaging<TEntity>> GetPageByModel(IQueryableModel<TEntity> model, int page = 1, int size = 10, CancellationToken cancellationToken = default)
        {
            return GetPage(model.ToQueryable(_db), page, size, cancellationToken);
        }
    }
}
