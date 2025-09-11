#pragma warning disable SA1401 // Fields should be private

using Microsoft.EntityFrameworkCore;
using TripleSix.Core.DataContext;
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

        /// <inheritdoc/>
        public IQueryable<TEntity> Query => _db.Set<TEntity>();

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
            var createdEntity = await Create(entity);

            var mapMethod = typeof(TResult).GetMethod(nameof(IMapFromEntityDto<TEntity>.MapFromEntity));
            if (mapMethod == null) return Mapper.MapData<TResult>(createdEntity);

            var result = Activator.CreateInstance<TResult>();
            var task = mapMethod.Invoke(result, [Mapper, ServiceProvider, createdEntity]) as Task;
            Task.WaitAll(task!);
            return result;
        }

        /// <inheritdoc/>
        public async Task<TEntity> CreateWithMapper(IMapToEntityDto<TEntity> input)
        {
            input.Validate(throwOnFailures: true);
            var entity = await input.MapToEntity(Mapper, ServiceProvider);

            return await Create(entity);
        }

        /// <inheritdoc/>
        public async Task<TResult> CreateWithMapper<TResult>(IMapToEntityDto<TEntity> input)
            where TResult : class
        {
            var entity = await CreateWithMapper(input);

            var mapMethod = typeof(TResult).GetMethod(nameof(IMapFromEntityDto<TEntity>.MapFromEntity));
            if (mapMethod == null) return Mapper.MapData<TResult>(entity);

            var result = Activator.CreateInstance<TResult>();
            var task = mapMethod.Invoke(result, [Mapper, ServiceProvider, entity]) as Task;
            Task.WaitAll(task!);
            return result;
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
            var updatedEntity = await Update(entity);

            var mapMethod = typeof(TResult).GetMethod(nameof(IMapFromEntityDto<TEntity>.MapFromEntity));
            if (mapMethod == null) return Mapper.MapData<TResult>(updatedEntity);

            var result = Activator.CreateInstance<TResult>();
            var task = mapMethod.Invoke(result, [Mapper, ServiceProvider, updatedEntity]) as Task;
            Task.WaitAll(task!);
            return result;
        }

        /// <inheritdoc/>
        public async Task<TEntity> UpdateWithMapper(TEntity entity, IMapToEntityDto<TEntity> input)
        {
            if (!input.IsAnyPropertyChanged()) return entity;

            input.Validate(throwOnFailures: true);
            var mappedEntity = await input.MapChangeEntity(Mapper, ServiceProvider, entity);
            return await Update(mappedEntity);
        }

        /// <inheritdoc/>
        public async Task<TResult> UpdateWithMapper<TResult>(TEntity entity, IMapToEntityDto<TEntity> input)
            where TResult : class
        {
            var updatedEntity = await UpdateWithMapper(entity, input);

            var mapMethod = typeof(TResult).GetMethod(nameof(IMapFromEntityDto<TEntity>.MapFromEntity));
            if (mapMethod == null) return Mapper.MapData<TResult>(updatedEntity);

            var result = Activator.CreateInstance<TResult>();
            var task = mapMethod.Invoke(result, [Mapper, ServiceProvider, updatedEntity]) as Task;
            Task.WaitAll(task!);
            return result;
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
        public async Task<TEntity?> GetFirstOrDefault(IQueryable<TEntity>? query = default)
        {
            using var activity = StartTraceMethodActivity();

            query ??= Query;
            return await query.FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<TResult?> GetFirstOrDefault<TResult>(IQueryable<TEntity>? query = default)
            where TResult : class
        {
            var entity = await GetFirstOrDefault(query);
            if (entity == null) return null;

            var mapMethod = typeof(TResult).GetMethod(nameof(IMapFromEntityDto<TEntity>.MapFromEntity));
            if (mapMethod == null) return Mapper.MapData<TResult>(entity);

            var result = Activator.CreateInstance<TResult>();
            var task = mapMethod.Invoke(result, [Mapper, ServiceProvider, entity]) as Task;
            Task.WaitAll(task!);
            return result;
        }

        /// <inheritdoc/>
        public async Task<TEntity?> GetFirstOrDefaultByQueryModel(IEntityQueryableDto<TEntity> model)
        {
            return await GetFirstOrDefault(await model.ToQueryable(Query, ServiceProvider));
        }

        /// <inheritdoc/>
        public async Task<TResult?> GetFirstOrDefaultByQueryModel<TResult>(IEntityQueryableDto<TEntity> model)
            where TResult : class
        {
            return await GetFirstOrDefault<TResult>(await model.ToQueryable(Query, ServiceProvider));
        }

        /// <inheritdoc/>
        public async Task<TEntity> GetFirst(IQueryable<TEntity>? query = default)
        {
            return await GetFirstOrDefault(query)
                ?? throw new NotFoundException<TEntity>();
        }

        /// <inheritdoc/>
        public async Task<TResult> GetFirst<TResult>(IQueryable<TEntity>? query = default)
            where TResult : class
        {
            return await GetFirstOrDefault<TResult>(query)
                ?? throw new NotFoundException<TEntity>();
        }

        /// <inheritdoc/>
        public async Task<TEntity> GetFirstByQueryModel(IEntityQueryableDto<TEntity> model)
        {
            return await GetFirst(await model.ToQueryable(Query, ServiceProvider));
        }

        /// <inheritdoc/>
        public async Task<TResult> GetFirstByQueryModel<TResult>(IEntityQueryableDto<TEntity> model)
            where TResult : class
        {
            return await GetFirst<TResult>(await model.ToQueryable(Query, ServiceProvider));
        }

        /// <inheritdoc/>
        public async Task<List<TEntity>> GetList(IQueryable<TEntity>? query = default)
        {
            using var activity = StartTraceMethodActivity();

            query ??= Query;
            return await query.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<List<TResult>> GetList<TResult>(IQueryable<TEntity>? query = default)
            where TResult : class
        {
            var entities = await GetList(query);

            var mapMethod = typeof(TResult).GetMethod(nameof(IMapFromEntityDto<TEntity>.MapFromEntity));
            if (mapMethod == null) return Mapper.MapData<List<TResult>>(entities);

            var tasks = new List<Task>();
            var items = new List<TResult>();
            foreach (var entity in entities)
            {
                var item = Activator.CreateInstance<TResult>();
                tasks.Add((mapMethod.Invoke(item, [Mapper, ServiceProvider, entity]) as Task)!);
                items.Add(item);
            }

            if (tasks.IsNotNullOrEmpty()) Task.WaitAll(tasks);
            return items;
        }

        /// <inheritdoc/>
        public async Task<List<TEntity>> GetListByQueryModel(IEntityQueryableDto<TEntity> model)
        {
            return await GetList(await model.ToQueryable(Query, ServiceProvider));
        }

        /// <inheritdoc/>
        public async Task<List<TResult>> GetListByQueryModel<TResult>(IEntityQueryableDto<TEntity> model)
            where TResult : class
        {
            return await GetList<TResult>(await model.ToQueryable(Query, ServiceProvider));
        }

        /// <inheritdoc/>
        public async Task<IPaging<TEntity>> GetPage(IQueryable<TEntity>? query = default, int page = 1, int size = 10)
        {
            using var activity = StartTraceMethodActivity();

            if (page <= 0) throw new ArgumentOutOfRangeException(nameof(page), "must be greater than 0");
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size), "must be greater than 0");

            query ??= Query;
            return new Paging<TEntity>(page, size)
            {
                Total = await query.LongCountAsync(),
                Items = await query.Skip((page - 1) * size).Take(size).ToListAsync(),
            };
        }

        /// <inheritdoc/>
        public async Task<IPaging<TResult>> GetPage<TResult>(IQueryable<TEntity>? query = default, int page = 1, int size = 10)
            where TResult : class
        {
            var entityResult = await GetPage(query, page, size);

            var mapMethod = typeof(TResult).GetMethod(nameof(IMapFromEntityDto<TEntity>.MapFromEntity));
            if (mapMethod == null)
            {
                return new Paging<TResult>(page, size)
                {
                    Total = entityResult.Total,
                    Items = Mapper.MapData<List<TResult>>(entityResult.Items),
                };
            }

            var tasks = new List<Task>();
            var items = new List<TResult>();
            foreach (var entity in entityResult.Items)
            {
                var item = Activator.CreateInstance<TResult>();
                tasks.Add((mapMethod.Invoke(item, [Mapper, ServiceProvider, entity]) as Task)!);
                items.Add(item);
            }

            if (tasks.IsNotNullOrEmpty()) Task.WaitAll(tasks);
            return new Paging<TResult>(page, size)
            {
                Total = entityResult.Total,
                Items = items,
            };
        }

        /// <inheritdoc/>
        public async Task<IPaging<TEntity>> GetPageByQueryModel(IEntityQueryableDto<TEntity> model, int page = 1, int size = 10)
        {
            return await GetPage(await model.ToQueryable(Query, ServiceProvider), page, size);
        }

        /// <inheritdoc/>
        public async Task<IPaging<TResult>> GetPageByQueryModel<TResult>(IEntityQueryableDto<TEntity> model, int page = 1, int size = 10)
            where TResult : class
        {
            return await GetPage<TResult>(await model.ToQueryable(Query, ServiceProvider), page, size);
        }

        /// <summary>
        /// Sự kiện khi entity được save changed thành công.
        /// </summary>
        /// <param name="entity">Entity xử lý.</param>
        /// <param name="event">Sự kiện gây thay đổi.</param>
        /// <returns>Task xử lý.</returns>
        protected virtual Task OnEntitySaveChanged(TEntity entity, EntityEvents @event)
        {
            return Task.CompletedTask;
        }
    }
}
