using TripleSix.Core.DataContext;
using TripleSix.Core.Entities;

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

        public async Task SoftDelete(TEntity entity, CancellationToken cancellationToken = default)
        {
            using var activity = StartTraceMethodActivity();

            entity.DeleteAt = DateTime.UtcNow;
            _db.Set<TEntity>().Update(entity);
            await _db.SaveChangesAsync(true, cancellationToken);
        }

        public async Task Restore(TEntity entity, CancellationToken cancellationToken = default)
        {
            using var activity = StartTraceMethodActivity();

            entity.DeleteAt = null;
            _db.Set<TEntity>().Update(entity);
            await _db.SaveChangesAsync(true, cancellationToken);
        }
    }
}
