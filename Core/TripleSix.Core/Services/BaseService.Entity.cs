using Microsoft.EntityFrameworkCore;
using TripleSix.Core.Entities.Interfaces;
using TripleSix.Core.Persistences.Interfaces;
using TripleSix.Core.Services.Interfaces;

namespace TripleSix.Core.Services
{
    /// <summary>
    /// Service cơ bản xử lý entity.
    /// </summary>
    /// <typeparam name="TEntity">Loại entity xử lý.</typeparam>
    public abstract class BaseService<TEntity> : BaseService, IService<TEntity>
        where TEntity : class, IEntity
    {
        /// <summary>
        /// Database context.
        /// </summary>
        public IDbDataContext DbContext { get; set; }

        /// <inheritdoc/>
        public virtual async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var result = await DbContext.Set<TEntity>()
                .AddAsync(entity, cancellationToken);

            await DbContext.SaveChangesAsync(cancellationToken);
            return result.Entity;
        }

        /// <inheritdoc/>
        public async Task<IList<TEntity>> GetList(CancellationToken cancellationToken = default)
        {
            var data = await DbContext.Set<TEntity>()
                .ToListAsync(cancellationToken);

            throw new NotImplementedException();
        }
    }
}
