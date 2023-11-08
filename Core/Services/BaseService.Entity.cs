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

        /// <summary>
        /// Câu query cơ bản.
        /// </summary>
        protected IQueryable<TEntity> Query => _db.Set<TEntity>();

        public virtual async Task<TEntity> Create(TEntity entity)
        {
            using var activity = StartTraceMethodActivity();

            var result = _db.Set<TEntity>().Add(entity);
            await _db.SaveChangesAsync(true);
            return result.Entity;
        }

        public async Task<TResult> Create<TResult>(TEntity entity)
            where TResult : class
        {
            var result = await Create(entity);
            return Mapper.MapData<TEntity, TResult>(result);
        }

        public async Task<TEntity> CreateWithMapper(IDto input, Action<TEntity>? afterMap = null)
        {
            input.Validate(throwOnFailures: true);
            input.Normalize();

            var entity = Mapper.MapData<IDto, TEntity>(input);
            afterMap?.Invoke(entity);

            return await Create(entity);
        }

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

        public virtual async Task<TEntity> Update(TEntity entity)
        {
            using var activity = StartTraceMethodActivity();

            var result = _db.Set<TEntity>().Update(entity);
            await _db.SaveChangesAsync(true);
            return result.Entity;
        }

        public async Task<TResult> Update<TResult>(TEntity entity)
            where TResult : class
        {
            var result = await Update(entity);
            return Mapper.MapData<TEntity, TResult>(result);
        }

        public async Task<TEntity> UpdateWithMapper(TEntity entity, IDto input, Action<TEntity>? afterMap = null)
        {
            if (!input.IsAnyPropertyChanged()) return entity;
            input.Validate(throwOnFailures: true);
            input.Normalize();

            Mapper.MapUpdate(input, entity);
            afterMap?.Invoke(entity);

            return await Update(entity);
        }

        public async Task<TResult> UpdateWithMapper<TResult>(TEntity entity, IDto input, Action<TEntity>? afterMap = null)
            where TResult : class
        {
            var result = await UpdateWithMapper(entity, input, afterMap);
            return Mapper.MapData<TEntity, TResult>(result);
        }

        public virtual async Task HardDelete(TEntity entity)
        {
            using var activity = StartTraceMethodActivity();

            _db.Set<TEntity>().Remove(entity);
            await _db.SaveChangesAsync(true);
        }

        public async Task<bool> Any(IQueryable<TEntity>? query = default)
        {
            using var activity = StartTraceMethodActivity();

            query ??= Query;
            return await query.AnyAsync();
        }

        public async Task<bool> AnyByQueryModel(IQueryableDto<TEntity> model)
        {
            return await Any(model.ToQueryable(Query));
        }

        public async Task<long> Count(IQueryable<TEntity>? query = default)
        {
            using var activity = StartTraceMethodActivity();

            query ??= Query;
            return await query.LongCountAsync();
        }

        public async Task<long> CountByQueryModel(IQueryableDto<TEntity> model)
        {
            return await Count(model.ToQueryable(Query));
        }

        public async Task<TResult?> GetFirstOrDefault<TResult>(IQueryable<TEntity>? query = default)
            where TResult : class
        {
            using var activity = StartTraceMethodActivity();

            query ??= Query;
            return await query.FirstOrDefaultAsync<TResult>(Mapper);
        }

        public async Task<TEntity?> GetFirstOrDefault(IQueryable<TEntity>? query = default)
        {
            return await GetFirstOrDefault<TEntity>(query);
        }

        public async Task<TResult?> GetFirstOrDefaultByQueryModel<TResult>(IQueryableDto<TEntity> model)
            where TResult : class
        {
            return await GetFirstOrDefault<TResult>(model.ToQueryable(Query));
        }

        public async Task<TEntity?> GetFirstOrDefaultByQueryModel(IQueryableDto<TEntity> model)
        {
            return await GetFirstOrDefault(model.ToQueryable(Query));
        }

        public async Task<TResult> GetFirst<TResult>(IQueryable<TEntity>? query = default)
            where TResult : class
        {
            var data = await GetFirstOrDefault<TResult>(query);
            if (data == null) throw new NotFoundException(typeof(TEntity));
            return data;
        }

        public async Task<TEntity> GetFirst(IQueryable<TEntity>? query = default)
        {
            return await GetFirst<TEntity>(query);
        }

        public async Task<TResult> GetFirstByQueryModel<TResult>(IQueryableDto<TEntity> model)
            where TResult : class
        {
            return await GetFirst<TResult>(model.ToQueryable(Query));
        }

        public async Task<TEntity> GetFirstByQueryModel(IQueryableDto<TEntity> model)
        {
            return await GetFirst(model.ToQueryable(Query));
        }

        public async Task<List<TResult>> GetList<TResult>(IQueryable<TEntity>? query = default)
            where TResult : class
        {
            using var activity = StartTraceMethodActivity();

            query ??= Query;
            return await query.ToListAsync<TResult>(Mapper);
        }

        public async Task<List<TEntity>> GetList(IQueryable<TEntity>? query = default)
        {
            return await GetList<TEntity>(query);
        }

        public async Task<List<TResult>> GetListByQueryModel<TResult>(IQueryableDto<TEntity> model)
            where TResult : class
        {
            return await GetList<TResult>(model.ToQueryable(Query));
        }

        public async Task<List<TEntity>> GetListByQueryModel(IQueryableDto<TEntity> model)
        {
            return await GetList(model.ToQueryable(Query));
        }

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

        public async Task<IPaging<TEntity>> GetPage(IQueryable<TEntity>? query = default, int page = 1, int size = 10)
        {
            return await GetPage<TEntity>(query, page, size);
        }

        public async Task<IPaging<TResult>> GetPageByQueryModel<TResult>(IQueryableDto<TEntity> model, int page = 1, int size = 10)
            where TResult : class
        {
            return await GetPage<TResult>(model.ToQueryable(Query), page, size);
        }

        public async Task<IPaging<TEntity>> GetPageByQueryModel(IQueryableDto<TEntity> model, int page = 1, int size = 10)
        {
            return await GetPage(model.ToQueryable(Query), page, size);
        }
    }
}
