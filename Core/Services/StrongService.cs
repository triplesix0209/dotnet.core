using TripleSix.Core.DataContext;
using TripleSix.Core.Entities;
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
        public async Task<TEntity> Update(Guid id, Action<TEntity> updateAction)
        {
            var entity = await GetFirstById(id);
            updateAction(entity);
            return await Update(entity);
        }

        /// <inheritdoc/>
        public async Task<TResult> Update<TResult>(Guid id, Action<TEntity> updateAction)
            where TResult : class
        {
            var entity = await GetFirstById(id);
            updateAction(entity);
            return await Update<TResult>(entity);
        }

        /// <inheritdoc/>
        public async Task<TEntity> UpdateWithMapper(Guid id, IMapToEntityDto<TEntity> input)
        {
            var entity = await GetFirstById(id);
            return await UpdateWithMapper(entity, input);
        }

        /// <inheritdoc/>
        public async Task<TResult> UpdateWithMapper<TResult>(Guid id, IMapToEntityDto<TEntity> input)
            where TResult : class
        {
            var entity = await GetFirstById(id);
            return await UpdateWithMapper<TResult>(entity, input);
        }

        /// <inheritdoc/>
        public async Task HardDelete(Guid id)
        {
            var entity = await GetFirstById(id);
            await HardDelete(entity);
        }

        /// <inheritdoc/>
        public virtual async Task<TEntity> SoftDelete(TEntity entity)
        {
            using var activity = StartTraceMethodActivity();

            entity.DeleteAt = DateTime.UtcNow;
            var result = _db.Set<TEntity>().Update(entity);
            await _db.SaveChangesAsync(true);
            await OnEntitySaveChanged(result.Entity, EntityEvents.SoftDeleted);

            return result.Entity;
        }

        /// <inheritdoc/>
        public async Task<TResult> SoftDelete<TResult>(TEntity entity)
            where TResult : class
        {
            var updatedEntity = await SoftDelete(entity);

            var mapMethod = typeof(TResult).GetMethod(nameof(IMapFromEntityDto<TEntity>.OnMapFromEntity));
            if (mapMethod == null) return Mapper.MapData<TResult>(updatedEntity);

            var result = Activator.CreateInstance<TResult>();
            var task = mapMethod.Invoke(result, [Mapper, ServiceProvider, updatedEntity]) as Task;
            Task.WaitAll(task!);
            return result;
        }

        /// <inheritdoc/>
        public async Task<TEntity> SoftDelete(Guid id)
        {
            var entity = await GetFirstById(id);
            return await SoftDelete(entity);
        }

        /// <inheritdoc/>
        public async Task<TResult> SoftDelete<TResult>(Guid id)
            where TResult : class
        {
            var entity = await GetFirstById(id);
            return await SoftDelete<TResult>(entity);
        }

        /// <inheritdoc/>
        public virtual async Task<TEntity> Restore(TEntity entity)
        {
            using var activity = StartTraceMethodActivity();

            entity.DeleteAt = null;
            var result = _db.Set<TEntity>().Update(entity);
            await _db.SaveChangesAsync(true);
            await OnEntitySaveChanged(result.Entity, EntityEvents.Restore);

            return result.Entity;
        }

        /// <inheritdoc/>
        public async Task<TResult> Restore<TResult>(TEntity entity)
            where TResult : class
        {
            var updatedEntity = await Restore(entity);

            var mapMethod = typeof(TResult).GetMethod(nameof(IMapFromEntityDto<TEntity>.OnMapFromEntity));
            if (mapMethod == null) return Mapper.MapData<TResult>(updatedEntity);

            var result = Activator.CreateInstance<TResult>();
            var task = mapMethod.Invoke(result, [Mapper, ServiceProvider, updatedEntity]) as Task;
            Task.WaitAll(task!);
            return result;
        }

        /// <inheritdoc/>
        public async Task<TEntity> Restore(Guid id)
        {
            var entity = await GetFirstById(id);
            return await Restore(entity);
        }

        /// <inheritdoc/>
        public async Task<TResult> Restore<TResult>(Guid id)
            where TResult : class
        {
            var entity = await GetFirstById(id);
            return await Restore<TResult>(entity);
        }

        /// <inheritdoc/>
        public async Task<TEntity?> GetFirstOrDefaultById(Guid id)
        {
            var query = Query.Where(x => x.Id == id);
            return await GetFirstOrDefault(query);
        }

        /// <inheritdoc/>
        public async Task<TResult?> GetFirstOrDefaultById<TResult>(Guid id)
            where TResult : class
        {
            var query = Query.Where(x => x.Id == id);
            return await GetFirstOrDefault<TResult>(query);
        }

        /// <inheritdoc/>
        public async Task<TEntity> GetFirstById(Guid id)
        {
            var query = Query.Where(x => x.Id == id);
            return await GetFirst(query);
        }

        /// <inheritdoc/>
        public async Task<TResult> GetFirstById<TResult>(Guid id)
            where TResult : class
        {
            var query = Query.Where(x => x.Id == id);
            return await GetFirst<TResult>(query);
        }
    }
}
