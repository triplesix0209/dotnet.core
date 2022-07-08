using TripleSix.Core.Entities.Interfaces;
using TripleSix.Core.Helpers;
using TripleSix.Core.Services.Interfaces;

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
        /// <inheritdoc/>
        public virtual Task<string?> GenerateCode(TEntity entity)
        {
            return Task.FromResult<string?>(null);
        }

        /// <summary>
        /// Khởi tạo entity kèm code tự phát sinh.
        /// </summary>
        /// <param name="entity"><inheritdoc/></param>
        /// <param name="cancellationToken"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public override sealed Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            return CreateAsync(entity, true, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task CreateAsync(TEntity entity, bool generateCode, CancellationToken cancellationToken = default)
        {
            if (generateCode && entity.Code.IsNullOrWhiteSpace())
                entity.Code = await GenerateCode(entity);

            await base.CreateAsync(entity, cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task SoftDeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await UpdateAsync(
                entity,
                e => { e.IsDeleted = true; },
                cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task RestoreAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await UpdateAsync(
                entity,
                e => { e.IsDeleted = false; },
                cancellationToken);
        }
    }
}
