#pragma warning disable SA1401 // Fields should be private

using System.Reflection;
using Elastic.Transport.Products.Elasticsearch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TripleSix.Core.DataContext;
using TripleSix.Core.Elastic;
using TripleSix.Core.Entities;
using TripleSix.Core.Exceptions;
using TripleSix.Core.Helpers;
using TripleSix.Core.Types;

namespace TripleSix.Core.Services
{
    /// <summary>
    /// Service cơ bản xử lý entity.
    /// </summary>
    /// <typeparam name="TEntity">Loại entity xử lý.</typeparam>
    public abstract class BaseService<TEntity> : BaseService,
        IService<TEntity>
        where TEntity : class, IEntity
    {
        internal readonly IDbDataContext _db;

        /// <summary>
        /// Khởi tạo <see cref="BaseService{TEntity}"/>.
        /// </summary>
        /// <param name="db"><see cref="IDbDataContext"/>.</param>
        public BaseService(IDbDataContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Câu query cơ bản.
        /// </summary>
        protected IQueryable<TEntity> Query => _db.Set<TEntity>();

        /// <inheritdoc/>
        public virtual async Task<TEntity> Create(TEntity entity)
        {
            using var activity = StartTraceMethodActivity();

            var result = _db.Set<TEntity>().Add(entity);
            await _db.SaveChangesAsync(true);
            await OnEntitySaveChanged(result.Entity, EntityEvents.Created);

            return result.Entity;
        }

        /// <inheritdoc/>
        public async Task<TResult> Create<TResult>(TEntity entity)
            where TResult : class
        {
            var result = await Create(entity);
            return Mapper.MapData<TEntity, TResult>(result);
        }

        /// <inheritdoc/>
        public async Task<TEntity> CreateWithMapper(IDto input, Action<TEntity>? afterMap = null)
        {
            input.Validate(throwOnFailures: true);
            input.Normalize();

            var entity = Mapper.MapData<IDto, TEntity>(input);
            afterMap?.Invoke(entity);

            return await Create(entity);
        }

        /// <inheritdoc/>
        public async Task<TResult> CreateWithMapper<TResult>(IDto input, Action<TEntity>? afterMap = null)
            where TResult : class
        {
            input.Validate(throwOnFailures: true);
            input.Normalize();

            var entity = Mapper.MapData<IDto, TEntity>(input);
            afterMap?.Invoke(entity);

            var result = await Create(entity);
            return Mapper.MapData<TEntity, TResult>(result);
        }

        /// <inheritdoc/>
        public virtual async Task<TEntity> Update(TEntity entity)
        {
            using var activity = StartTraceMethodActivity();

            var result = _db.Set<TEntity>().Update(entity);
            await _db.SaveChangesAsync(true);
            await OnEntitySaveChanged(result.Entity, EntityEvents.Updated);

            return result.Entity;
        }

        /// <inheritdoc/>
        public async Task<TResult> Update<TResult>(TEntity entity)
            where TResult : class
        {
            var result = await Update(entity);
            return Mapper.MapData<TEntity, TResult>(result);
        }

        /// <inheritdoc/>
        public async Task<TEntity> UpdateWithMapper(TEntity entity, IDto input, Action<TEntity>? afterMap = null)
        {
            if (!input.IsAnyPropertyChanged()) return entity;
            input.Validate(throwOnFailures: true);
            input.Normalize();

            Mapper.MapUpdate(input, entity);
            afterMap?.Invoke(entity);

            return await Update(entity);
        }

        /// <inheritdoc/>
        public async Task<TResult> UpdateWithMapper<TResult>(TEntity entity, IDto input, Action<TEntity>? afterMap = null)
            where TResult : class
        {
            var result = await UpdateWithMapper(entity, input, afterMap);
            return Mapper.MapData<TEntity, TResult>(result);
        }

        /// <inheritdoc/>
        public virtual async Task HardDelete(TEntity entity)
        {
            using var activity = StartTraceMethodActivity();

            _db.Set<TEntity>().Remove(entity);
            await _db.SaveChangesAsync(true);
            await OnEntitySaveChanged(entity, EntityEvents.HardDeleted);
        }

        /// <inheritdoc/>
        public async Task<bool> Any(IQueryable<TEntity>? query = default)
        {
            using var activity = StartTraceMethodActivity();

            query ??= Query;
            return await query.AnyAsync();
        }

        /// <inheritdoc/>
        public async Task<bool> AnyByQueryModel(IEntityQueryableDto<TEntity> model)
        {
            return await Any(await model.ToQueryable(Query, ServiceProvider));
        }

        /// <inheritdoc/>
        public async Task<long> Count(IQueryable<TEntity>? query = default)
        {
            using var activity = StartTraceMethodActivity();

            query ??= Query;
            return await query.LongCountAsync();
        }

        /// <inheritdoc/>
        public async Task<long> CountByQueryModel(IEntityQueryableDto<TEntity> model)
        {
            return await Count(await model.ToQueryable(Query, ServiceProvider));
        }

        /// <inheritdoc/>
        public async Task<TResult?> GetFirstOrDefault<TResult>(IQueryable<TEntity>? query = default)
            where TResult : class
        {
            using var activity = StartTraceMethodActivity();

            query ??= Query;
            var entity = await query.FirstOrDefaultAsync();
            if (entity == null) return null;

            var result = Mapper.MapData<TResult>(entity);
            if (result is IDto dto)
                await dto.AfterGetFirst(entity, ServiceProvider);

            return result;
        }

        /// <inheritdoc/>
        public async Task<TEntity?> GetFirstOrDefault(IQueryable<TEntity>? query = default)
        {
            return await GetFirstOrDefault<TEntity>(query);
        }

        /// <inheritdoc/>
        public async Task<TResult?> GetFirstOrDefaultByQueryModel<TResult>(IEntityQueryableDto<TEntity> model)
            where TResult : class
        {
            return await GetFirstOrDefault<TResult>(await model.ToQueryable(Query, ServiceProvider));
        }

        /// <inheritdoc/>
        public async Task<TEntity?> GetFirstOrDefaultByQueryModel(IEntityQueryableDto<TEntity> model)
        {
            return await GetFirstOrDefault(await model.ToQueryable(Query, ServiceProvider));
        }

        /// <inheritdoc/>
        public async Task<TResult> GetFirst<TResult>(IQueryable<TEntity>? query = default)
            where TResult : class
        {
            var data = await GetFirstOrDefault<TResult>(query)
                ?? throw new NotFoundException<TEntity>();
            return data;
        }

        /// <inheritdoc/>
        public async Task<TEntity> GetFirst(IQueryable<TEntity>? query = default)
        {
            return await GetFirst<TEntity>(query);
        }

        /// <inheritdoc/>
        public async Task<TResult> GetFirstByQueryModel<TResult>(IEntityQueryableDto<TEntity> model)
            where TResult : class
        {
            return await GetFirst<TResult>(await model.ToQueryable(Query, ServiceProvider));
        }

        /// <inheritdoc/>
        public async Task<TEntity> GetFirstByQueryModel(IEntityQueryableDto<TEntity> model)
        {
            return await GetFirst(await model.ToQueryable(Query, ServiceProvider));
        }

        /// <inheritdoc/>
        public async Task<List<TResult>> GetList<TResult>(IQueryable<TEntity>? query = default)
            where TResult : class
        {
            using var activity = StartTraceMethodActivity();

            query ??= Query;
            return await query.ToListAsync<TResult>(Mapper);
        }

        /// <inheritdoc/>
        public async Task<List<TEntity>> GetList(IQueryable<TEntity>? query = default)
        {
            return await GetList<TEntity>(query);
        }

        /// <inheritdoc/>
        public async Task<List<TResult>> GetListByQueryModel<TResult>(IEntityQueryableDto<TEntity> model)
            where TResult : class
        {
            return await GetList<TResult>(await model.ToQueryable(Query, ServiceProvider));
        }

        /// <inheritdoc/>
        public async Task<List<TEntity>> GetListByQueryModel(IEntityQueryableDto<TEntity> model)
        {
            return await GetList(await model.ToQueryable(Query, ServiceProvider));
        }

        /// <inheritdoc/>
        public async Task<IPaging<TResult>> GetPage<TResult>(IQueryable<TEntity>? query = default, int page = 1, int size = 10)
            where TResult : class
        {
            using var activity = StartTraceMethodActivity();

            if (page <= 0) throw new ArgumentOutOfRangeException(nameof(page), "must be greater than 0");
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size), "must be greater than 0");

            query ??= Query;
            var result = new Paging<TResult>(page, size)
            {
                Total = await query.LongCountAsync(),
                Items = await query
                    .Skip((page - 1) * size)
                    .Take(size)
                    .ToListAsync<TResult>(Mapper),
            };
            return result;
        }

        /// <inheritdoc/>
        public async Task<IPaging<TEntity>> GetPage(IQueryable<TEntity>? query = default, int page = 1, int size = 10)
        {
            return await GetPage<TEntity>(query, page, size);
        }

        /// <inheritdoc/>
        public async Task<IPaging<TResult>> GetPageByQueryModel<TResult>(IEntityQueryableDto<TEntity> model, int page = 1, int size = 10)
            where TResult : class
        {
            return await GetPage<TResult>(await model.ToQueryable(Query, ServiceProvider), page, size);
        }

        /// <inheritdoc/>
        public async Task<IPaging<TEntity>> GetPageByQueryModel(IEntityQueryableDto<TEntity> model, int page = 1, int size = 10)
        {
            return await GetPage(await model.ToQueryable(Query, ServiceProvider), page, size);
        }

        /// <summary>
        /// Hàm xử lý tự động đồng bộ dữ liệu với elasticsearch.
        /// </summary>
        /// <param name="entity">Entity xử lý.</param>
        /// <param name="event">Sự kiện gây thay đổi.</param>
        /// <returns>Task xử lý.</returns>
        protected virtual async Task AutoSyncElastic(TEntity entity, EntityEvents @event)
        {
            if (!(@event == EntityEvents.Created
                || @event == EntityEvents.Updated
                || @event == EntityEvents.HardDeleted))
                return;

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
                if (@event == EntityEvents.HardDeleted) response = await document.Delete(client);
                else response = await document.Index(client);

                if (response != null && response.IsValidResponse == false)
                {
                    Logger.LogError($"Auto sync elastic [{document.GetIndexName()}] failed"
                        + (response.ElasticsearchServerError != null ? "\n" + response.ElasticsearchServerError.Error.Reason : string.Empty));
                }
            }
        }

        /// <summary>
        /// Sự kiện khi entity được save changed thành công.
        /// </summary>
        /// <param name="entity">Entity xử lý.</param>
        /// <param name="event">Sự kiện gây thay đổi.</param>
        /// <returns>Task xử lý.</returns>
        protected virtual async Task OnEntitySaveChanged(TEntity entity, EntityEvents @event)
        {
            await AutoSyncElastic(entity, @event);
        }
    }
}
