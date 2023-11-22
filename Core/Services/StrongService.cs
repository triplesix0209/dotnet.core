using System.Reflection;
using Elastic.Transport.Products.Elasticsearch;
using Microsoft.Extensions.Logging;
using TripleSix.Core.DataContext;
using TripleSix.Core.Elastic;
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
            var result = await Update(id, updateAction);
            return Mapper.MapData<TEntity, TResult>(result);
        }

        /// <inheritdoc/>
        public async Task<TEntity> UpdateWithMapper(Guid id, IDto input, Action<TEntity>? afterMap = null)
        {
            var entity = await GetFirstById(id);
            return await UpdateWithMapper(entity, input, afterMap);
        }

        /// <inheritdoc/>
        public async Task<TResult> UpdateWithMapper<TResult>(Guid id, IDto input, Action<TEntity>? afterMap = null)
            where TResult : class
        {
            var entity = await GetFirstById(id);
            return await UpdateWithMapper<TResult>(entity, input, afterMap);
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
            var result = await SoftDelete(entity);
            return Mapper.MapData<TEntity, TResult>(result);
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
            var result = await Restore(entity);
            return Mapper.MapData<TEntity, TResult>(result);
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
        public async Task<TResult?> GetFirstOrDefaultById<TResult>(Guid id)
            where TResult : class
        {
            var query = Query.Where(x => x.Id == id);
            return await GetFirstOrDefault<TResult>(query);
        }

        /// <inheritdoc/>
        public async Task<TEntity?> GetFirstOrDefaultById(Guid id)
        {
            var query = Query.Where(x => x.Id == id);
            return await GetFirstOrDefault(query);
        }

        /// <inheritdoc/>
        public async Task<TResult> GetFirstById<TResult>(Guid id)
            where TResult : class
        {
            var query = Query.Where(x => x.Id == id);
            return await GetFirst<TResult>(query);
        }

        /// <inheritdoc/>
        public async Task<TEntity> GetFirstById(Guid id)
        {
            var query = Query.Where(x => x.Id == id);
            return await GetFirst(query);
        }

        /// <inheritdoc/>
        protected override async Task AutoSyncElastic(TEntity entity, EntityEvents @event)
        {
            var elasticAutoSyncAttrs = GetType().GetCustomAttributes(typeof(ElasticAutoSyncAttribute<>));
            if (elasticAutoSyncAttrs.IsNullOrEmpty()) return;

            var client = Extension.CreateElasticsearchClient(Configuration);
            foreach (var elasticAutoSyncAttr in elasticAutoSyncAttrs)
            {
                var documentType = elasticAutoSyncAttr.GetType().GetGenericArguments()[0];
                var elasticDocumentAttr = documentType.GetCustomAttribute<ElasticDocumentAttribute>();
                if (elasticDocumentAttr == null) continue;
                if (Mapper.MapData(entity, typeof(TEntity), documentType) is not IElasticDocument document) continue;

                ElasticsearchResponse? response;
                if (@event == EntityEvents.SoftDeleted || @event == EntityEvents.HardDeleted)
                    response = await document.Delete(client);
                else if ((@event == EntityEvents.Created || @event == EntityEvents.Updated || @event == EntityEvents.Restore)
                    && entity.DeleteAt == null)
                    response = await document.Index(client);
                else continue;

                if (response != null && response.IsValidResponse == false)
                {
                    Logger.LogError($"Auto sync elastic [{document.GetIndexName()}] failed"
                        + (response.ElasticsearchServerError != null ? "\n" + response.ElasticsearchServerError.Error.Reason : string.Empty));
                }
            }
        }
    }
}
