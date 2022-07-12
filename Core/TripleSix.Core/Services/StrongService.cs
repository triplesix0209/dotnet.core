using TripleSix.Core.Entities;
using TripleSix.Core.Helpers;

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
        /// Khởi tạo entity kèm code tự phát sinh.
        /// </summary>
        /// <param name="entity"><inheritdoc/></param>
        /// <param name="cancellationToken"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public override sealed Task<TEntity> Create(TEntity entity, CancellationToken cancellationToken = default)
        {
            return Create(entity, true, cancellationToken);
        }

        /// <inheritdoc/>
        public virtual Task<string?> GenerateCode(TEntity? entity = default, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<string?>(null);
        }

        /// <inheritdoc/>
        public virtual async Task<TEntity> Create(TEntity entity, bool generateCode, CancellationToken cancellationToken = default)
        {
            // tự phát sinh mã nếu không được nhập
            if (generateCode && entity.Code.IsNullOrWhiteSpace())
            {
                entity.Code = await GenerateCode(entity);
                if (entity.Code.IsNullOrWhiteSpace()) entity.Code = null;
            }

            return await base.Create(entity, cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task SoftDelete(TEntity entity, CancellationToken cancellationToken = default)
        {
            await Update(
                entity,
                e => { e.IsDeleted = true; },
                cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task Restore(TEntity entity, CancellationToken cancellationToken = default)
        {
            await Update(
                entity,
                e => { e.IsDeleted = false; },
                cancellationToken);
        }

        /// <inheritdoc/>
        public Task<bool> Any(bool includeDeleted, CancellationToken cancellationToken = default)
        {
            var query = DbContext.Set<TEntity>()
                .WhereIf(includeDeleted == false, x => !x.IsDeleted);
            return Any(query, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<long> Count(bool includeDeleted, CancellationToken cancellationToken = default)
        {
            var query = DbContext.Set<TEntity>()
                .WhereIf(includeDeleted == false, x => !x.IsDeleted);
            return Count(query, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<TEntity?> GetFirstOrDefault(Guid id, bool includeDeleted, CancellationToken cancellationToken = default)
        {
            var query = DbContext.Set<TEntity>()
                .WhereIf(includeDeleted == false, x => !x.IsDeleted)
                .Where(x => x.Id == id);
            return GetFirstOrDefault(query, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<TResult?> GetFirstOrDefault<TResult>(Guid id, bool includeDeleted, CancellationToken cancellationToken = default)
            where TResult : class
        {
            var result = await GetFirstOrDefault(id, includeDeleted, cancellationToken);
            return Mapper.Map<TResult>(result);
        }

        /// <inheritdoc/>
        public Task<TEntity> GetFirst(Guid id, bool includeDeleted, CancellationToken cancellationToken = default)
        {
            var query = DbContext.Set<TEntity>()
                .WhereIf(includeDeleted == false, x => !x.IsDeleted)
                .Where(x => x.Id == id);
            return GetFirst(query, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<TResult> GetFirst<TResult>(Guid id, bool includeDeleted, CancellationToken cancellationToken = default)
            where TResult : class
        {
            var result = await GetFirst(id, includeDeleted, cancellationToken);
            return Mapper.Map<TResult>(result);
        }
    }
}
