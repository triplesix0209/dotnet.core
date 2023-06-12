using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TripleSix.Core.DataContext;
using TripleSix.Core.Entities;
using TripleSix.Core.Exceptions;
using TripleSix.Core.Helpers;
using TripleSix.Core.Types;

namespace TripleSix.Core.Services
{
    /// <summary>
    /// Service xử lý strong entity.
    /// </summary>
    /// <typeparam name="TEntity">Loại entity xử lý.</typeparam>
    public abstract class StrongService<TEntity> : BaseService<TEntity>,
        IStrongService<TEntity>
        where TEntity : class, IStrongEntity
    {
        /// <summary>
        /// Khởi tạo <see cref="StrongService{TEntity}"/>.
        /// </summary>
        /// <param name="db"><see cref="IDbDataContext"/>.</param>
        protected StrongService(IDbDataContext db)
            : base(db)
        {
        }

        /// <inheritdoc/>
        public async Task Update(Guid id, bool includeDeleted, Action<TEntity> updateMethod, CancellationToken cancellationToken = default)
        {
            var entity = await GetById(id, includeDeleted, cancellationToken);
            await Update(entity, updateMethod, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task UpdateWithMapper(Guid id, bool includeDeleted, IDto input, CancellationToken cancellationToken = default)
        {
            var entity = await GetById(id, includeDeleted, cancellationToken);
            await UpdateWithMapper(entity, input, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task HardDelete(Guid id, bool includeDeleted, CancellationToken cancellationToken = default)
        {
            var entity = await GetById(id, includeDeleted, cancellationToken);
            await HardDelete(entity, cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task SoftDelete(TEntity entity, CancellationToken cancellationToken = default)
        {
            using var activity = StartTraceMethodActivity();

            entity.DeleteAt = DateTime.UtcNow;

            _db.Set<TEntity>().Update(entity);
            await _db.SaveChangesAsync(true, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task SoftDelete(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await GetById(id, true, cancellationToken);
            await SoftDelete(entity, cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task Restore(TEntity entity, CancellationToken cancellationToken = default)
        {
            using var activity = StartTraceMethodActivity();

            entity.DeleteAt = null;

            _db.Set<TEntity>().Update(entity);
            await _db.SaveChangesAsync(true, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task Restore(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await GetById(id, true, cancellationToken);
            await Restore(entity, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<bool> Any(bool includeDeleted, CancellationToken cancellationToken = default)
        {
            var query = Query
                .WhereIf(includeDeleted == false, x => x.DeleteAt == null);

            return await Any(query, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<long> Count(bool includeDeleted, CancellationToken cancellationToken = default)
        {
            var query = Query
                .WhereIf(includeDeleted == false, x => x.DeleteAt == null);

            return await Count(query, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<TResult?> GetOrDefaultById<TResult>(Guid id, bool includeDeleted, CancellationToken cancellationToken = default)
            where TResult : class
        {
            using var activity = StartTraceMethodActivity();

            var query = Query
                .WhereIf(includeDeleted == false, x => x.DeleteAt == null)
                .Where(x => x.Id == id);

            if (typeof(TResult) == typeof(TEntity))
                return await query.SingleOrDefaultAsync(cancellationToken) as TResult;
            if (!CanConvertEntityToModel<TResult>())
                return await query.ProjectTo<TResult>(Mapper.ConfigurationProvider).SingleOrDefaultAsync(cancellationToken);

            var data = await query.SingleOrDefaultAsync(cancellationToken);
            if (data == null) return null;
            return await ConvertEntityToModel<TResult>(data, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<TEntity?> GetOrDefaultById(Guid id, bool includeDeleted, CancellationToken cancellationToken = default)
        {
            return await GetOrDefaultById<TEntity>(id, includeDeleted, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<TResult> GetById<TResult>(Guid id, bool includeDeleted, CancellationToken cancellationToken = default)
            where TResult : class
        {
            var data = await GetOrDefaultById<TResult>(id, includeDeleted, cancellationToken);
            if (data == null) throw new EntityNotFoundException(typeof(TEntity));

            return data;
        }

        /// <inheritdoc/>
        public async Task<TEntity> GetById(Guid id, bool includeDeleted, CancellationToken cancellationToken = default)
        {
            return await GetById<TEntity>(id, includeDeleted, cancellationToken);
        }
    }
}
