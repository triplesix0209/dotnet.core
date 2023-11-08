﻿using TripleSix.Core.DataContext;
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

        public async Task<TEntity> Update(Guid id, Action<TEntity> updateAction)
        {
            var entity = await GetFirstById(id);
            updateAction(entity);

            return await Update(entity);
        }

        public async Task<TResult> Update<TResult>(Guid id, Action<TEntity> updateAction)
            where TResult : class
        {
            var result = await Update(id, updateAction);
            return Mapper.MapData<TEntity, TResult>(result);
        }

        public async Task<TEntity> UpdateWithMapper(Guid id, IDto input, Action<TEntity>? afterMap = null)
        {
            var entity = await GetFirstById(id);
            return await UpdateWithMapper(entity, input, afterMap);
        }

        public async Task<TResult> UpdateWithMapper<TResult>(Guid id, IDto input, Action<TEntity>? afterMap = null)
            where TResult : class
        {
            var entity = await GetFirstById(id);
            return await UpdateWithMapper<TResult>(entity, input, afterMap);
        }

        public async Task HardDelete(Guid id)
        {
            var entity = await GetFirstById(id);
            await HardDelete(entity);
        }

        public virtual async Task<TEntity> SoftDelete(TEntity entity)
        {
            using var activity = StartTraceMethodActivity();

            entity.DeleteAt = DateTime.UtcNow;
            var result = _db.Set<TEntity>().Update(entity);
            await _db.SaveChangesAsync(true);

            return result.Entity;
        }

        public async Task<TResult> SoftDelete<TResult>(TEntity entity)
            where TResult : class
        {
            var result = await SoftDelete(entity);
            return Mapper.MapData<TEntity, TResult>(result);
        }

        public async Task<TEntity> SoftDelete(Guid id)
        {
            var entity = await GetFirstById(id);
            return await SoftDelete(entity);
        }

        public async Task<TResult> SoftDelete<TResult>(Guid id)
            where TResult : class
        {
            var entity = await GetFirstById(id);
            return await SoftDelete<TResult>(entity);
        }

        public virtual async Task<TEntity> Restore(TEntity entity)
        {
            using var activity = StartTraceMethodActivity();

            entity.DeleteAt = null;
            var result = _db.Set<TEntity>().Update(entity);
            await _db.SaveChangesAsync(true);

            return result.Entity;
        }

        public async Task<TResult> Restore<TResult>(TEntity entity)
            where TResult : class
        {
            var result = await Restore(entity);
            return Mapper.MapData<TEntity, TResult>(result);
        }

        public async Task<TEntity> Restore(Guid id)
        {
            var entity = await GetFirstById(id);
            return await Restore(entity);
        }

        public async Task<TResult> Restore<TResult>(Guid id)
            where TResult : class
        {
            var entity = await GetFirstById(id);
            return await Restore<TResult>(entity);
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
