using AutoMapper;
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
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        /// <summary>
        /// Database context.
        /// </summary>
        public IDbDataContext Db { get; set; }

        /// <summary>
        /// Automapper.
        /// </summary>
        public IMapper Mapper { get; set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        /// <inheritdoc/>
        public virtual async Task<TEntity> Create(TEntity entity, CancellationToken cancellationToken = default)
        {
            await Db.Set<TEntity>()
                .AddAsync(entity, cancellationToken);

            await Db.SaveChangesAsync(cancellationToken);
            return entity;
        }

        /// <inheritdoc/>
        public async Task<TResult> CreateWithMapper<TResult>(IDataDto input, CancellationToken cancellationToken = default)
            where TResult : class
        {
            var entity = Mapper.MapData<IDataDto, TEntity>(input);
            var result = await Create(entity, cancellationToken);
            return Mapper.MapData<TEntity, TResult>(result);
        }

        /// <inheritdoc/>
        public virtual async Task Update(TEntity entity, Action<TEntity> updateMethod, CancellationToken cancellationToken = default)
        {
            updateMethod(entity);
            Db.Set<TEntity>().Update(entity);

            await Db.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task UpdateWithMapper(TEntity entity, IDataDto input, CancellationToken cancellationToken = default)
        {
            if (!input.IsAnyPropertyChanged()) return;

            await Update(
                entity,
                e => Mapper.MapUpdate(input, e),
                cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task Delete(TEntity entity, CancellationToken cancellationToken = default)
        {
            Db.Set<TEntity>().Remove(entity);

            await Db.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public virtual Task<bool> Any(IQueryable<TEntity>? query = default, CancellationToken cancellationToken = default)
        {
            if (query == null) query = Db.Set<TEntity>();
            return query.AnyAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public virtual Task<long> Count(IQueryable<TEntity>? query = default, CancellationToken cancellationToken = default)
        {
            if (query == null) query = Db.Set<TEntity>();
            return query.LongCountAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public virtual Task<TEntity?> GetFirstOrDefault(IQueryable<TEntity>? query = default, CancellationToken cancellationToken = default)
        {
            if (query == null) query = Db.Set<TEntity>();
            return query.FirstOrDefaultAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<TResult?> GetFirstOrDefault<TResult>(IQueryable<TEntity>? query = default, CancellationToken cancellationToken = default)
            where TResult : class
        {
            var result = await GetFirstOrDefault(query, cancellationToken);
            return result == null ? null : Mapper.MapData<TResult>(result);
        }

        /// <inheritdoc/>
        public async Task<TEntity> GetFirst(IQueryable<TEntity>? query = default, CancellationToken cancellationToken = default)
        {
            var data = await GetFirstOrDefault(query, cancellationToken);
            if (data == null)
                throw new EntityNotFoundException(typeof(TEntity), query);

            return data;
        }

        /// <inheritdoc/>
        public async Task<TResult> GetFirst<TResult>(IQueryable<TEntity>? query = default, CancellationToken cancellationToken = default)
            where TResult : class
        {
            var result = await GetFirst(query, cancellationToken);
            return Mapper.MapData<TResult>(result);
        }

        /// <inheritdoc/>
        public virtual Task<List<TEntity>> GetList(IQueryable<TEntity>? query = default, CancellationToken cancellationToken = default)
        {
            if (query == null) query = Db.Set<TEntity>();
            return query.ToListAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<List<TResult>> GetList<TResult>(IQueryable<TEntity>? query = default, CancellationToken cancellationToken = default)
            where TResult : class
        {
            var result = await GetList(query, cancellationToken);
            return Mapper.MapData<List<TResult>>(result);
        }

        /// <inheritdoc/>
        public virtual async Task<IPaging<TEntity>> GetPage(IQueryable<TEntity>? query = default, int page = 1, int size = 10, CancellationToken cancellationToken = default)
        {
            if (page <= 0) throw new ArgumentOutOfRangeException(nameof(page), "must be greater than 0");
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size), "must be greater than 0");
            if (query == null) query = Db.Set<TEntity>();

            var total = await Count(query, cancellationToken);
            var items = total == 0
                ? new List<TEntity>()
                : await query
                    .Skip((page - 1) * size)
                    .Take(size)
                    .ToListAsync(cancellationToken);

            return new Paging<TEntity>(items, total, page, size);
        }

        /// <inheritdoc/>
        public async Task<IPaging<TResult>> GetPage<TResult>(IQueryable<TEntity>? query = default, int page = 1, int size = 10, CancellationToken cancellationToken = default)
            where TResult : class
        {
            var result = await GetPage(query, page, size, cancellationToken);
            var items = Mapper.MapData<List<TResult>>(result.Items);
            return new Paging<TResult>(
                items,
                result.Total,
                result.Page,
                result.Size);
        }
    }
}
