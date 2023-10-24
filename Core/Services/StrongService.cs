using TripleSix.Core.DataContext;
using TripleSix.Core.Entities;
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

        public async Task Update(Guid id, Action<TEntity> updateMethod)
        {
            var entity = await GetFirstById(id);
            await Update(entity, updateMethod);
        }

        public async Task UpdateWithMapper(Guid id, IDto input)
        {
            input.Normalize();
            var entity = await GetFirstById(id);
            await UpdateWithMapper(entity, input);
        }

        public async Task HardDelete(Guid id)
        {
            var entity = await GetFirstById(id);
            await HardDelete(entity);
        }

        public async Task SoftDelete(TEntity entity)
        {
            using var activity = StartTraceMethodActivity();

            entity.DeleteAt = DateTime.UtcNow;
            _db.Set<TEntity>().Update(entity);
            await _db.SaveChangesAsync(true);
        }

        public async Task SoftDelete(Guid id)
        {
            var entity = await GetFirstById(id);
            await SoftDelete(entity);
        }

        public async Task Restore(TEntity entity)
        {
            using var activity = StartTraceMethodActivity();

            entity.DeleteAt = null;
            _db.Set<TEntity>().Update(entity);
            await _db.SaveChangesAsync(true);
        }

        public async Task Restore(Guid id)
        {
            var entity = await GetFirstById(id);
            await Restore(entity);
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
    }
}
