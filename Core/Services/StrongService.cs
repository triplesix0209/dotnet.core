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

        public async Task<TResult?> GetFirstOrDefaultById<TResult>(Guid id)
            where TResult : class
        {
            var query = Query.Where(x => x.Id == id);
            return await GetFirstOrDefault<TResult>(query);
        }

        public async Task<TEntity?> GetFirstOrDefaultById(Guid id)
        {
            var query = Query.Where(x => x.Id == id);
            return await GetFirstOrDefault(query);
        }

        public async Task<TResult> GetFirstById<TResult>(Guid id)
            where TResult : class
        {
            var query = Query.Where(x => x.Id == id);
            return await GetFirst<TResult>(query);
        }

        public async Task<TEntity> GetFirstById(Guid id)
        {
            var query = Query.Where(x => x.Id == id);
            return await GetFirst(query);
        }

        public async Task SoftDelete(TEntity entity)
        {
            using var activity = StartTraceMethodActivity();

            entity.DeleteAt = DateTime.UtcNow;
            _db.Set<TEntity>().Update(entity);
            await _db.SaveChangesAsync(true);
        }

        public async Task Restore(TEntity entity)
        {
            using var activity = StartTraceMethodActivity();

            entity.DeleteAt = null;
            _db.Set<TEntity>().Update(entity);
            await _db.SaveChangesAsync(true);
        }
    }
}
