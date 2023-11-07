#pragma warning disable SA1401 // Fields should be private

using Autofac;
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

            _db.Set<TEntity>().Add(entity);
            await _db.SaveChangesAsync(true);
            return entity;
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

        public virtual async Task Update(TEntity entity, Action<TEntity> updateMethod)
        {
            using var activity = StartTraceMethodActivity();

            updateMethod(entity);
            _db.Set<TEntity>().Update(entity);
            await _db.SaveChangesAsync(true);
        }

        public async Task UpdateWithMapper(TEntity entity, IDto input, Action<TEntity>? afterMap = null)
        {
            if (!input.IsAnyPropertyChanged()) return;
            input.Validate(throwOnFailures: true);
            input.Normalize();

            await Update(entity, e =>
            {
                Mapper.MapUpdate(input, e);
                afterMap?.Invoke(e);
            });
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
            if (!CanConvertEntityToModel<TResult>())
                return await query.FirstOrDefaultAsync<TResult>(Mapper);

            var data = await query.FirstOrDefaultAsync();
            if (data == null) return null;
            return await ConvertEntityToModel<TResult>(data);
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
            if (!CanConvertEntityToModel<TResult>())
                return await query.ToListAsync<TResult>(Mapper);

            var data = await query.ToListAsync();
            return await ConvertEntityToModel<TResult>(data);
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
            };

            if (!CanConvertEntityToModel<TResult>())
            {
                result.Items = await query
                    .Skip((page - 1) * size)
                    .Take(size)
                    .ToListAsync<TResult>(Mapper);
            }
            else
            {
                var data = await query
                    .Skip((page - 1) * size)
                    .Take(size)
                    .ToListAsync();
                result.Items = await ConvertEntityToModel<TResult>(data);
            }

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

        /// <summary>
        /// Kiểm tra có thể chuyển đổi dữ liệu entity sang kiểu chỉ định không.
        /// </summary>
        /// <typeparam name="TModel">Kiểu dữ liệu đầu ra.</typeparam>
        /// <returns><c>True</c> nếu có thể, ngược lại là <c>False</c>.</returns>
        protected bool CanConvertEntityToModel<TModel>()
        {
            if (!typeof(TModel).IsAssignableTo<IDto>()) return false;
            var readInterface = typeof(IReadableWithModel<,>).MakeGenericType(typeof(TEntity), typeof(TModel));
            return GetType().IsAssignableTo(readInterface);
        }

        /// <summary>
        /// Chuyển đổi danh sách dữ liệu entity sang kiểu chỉ định.
        /// </summary>
        /// <typeparam name="TModel">Kiểu dữ liệu đầu ra.</typeparam>
        /// <param name="entities">Danh sách entity sẽ chuyển đổi.</param>
        /// <returns>Danh sách dữ liệu sau khi chuyển đổi.</returns>
        protected async Task<List<TModel>> ConvertEntityToModel<TModel>(List<TEntity> entities)
        {
            var readInterface = typeof(IReadableWithModel<,>).MakeGenericType(typeof(TEntity), typeof(TModel));
            var method = readInterface.GetMethod(nameof(IReadableWithModel<IEntity, IDto>.ConvertEntityToModel))
                ?? throw new Exception($"{GetType().Name} need implement {nameof(IReadableWithModel<IEntity, IDto>)}<{typeof(TEntity).Name}, {typeof(TModel).Name}>");

            var result = new List<TModel>();
            foreach (var entity in entities)
            {
                var input = new object?[] { entity, null };
                if (method.Invoke(this, input) is not Task task)
                    throw new Exception($"Error when convert {typeof(TEntity).Name} to {typeof(TModel).FullName}");
                await task.WaitAsync(CancellationToken.None);

                if (input[1] == null)
                    throw new Exception($"Error to convert {typeof(TEntity).Name} to {typeof(TModel).FullName}");
                result.Add((TModel)input[1]!);
            }

            return result;
        }

        /// <summary>
        /// Chuyển đổi dữ liệu entity sang kiểu chỉ định.
        /// </summary>
        /// <typeparam name="TModel">Kiểu dữ liệu đầu ra.</typeparam>
        /// <param name="entity">entity sẽ chuyển đổi.</param>
        /// <returns>Dữ liệu sau khi chuyển đổi.</returns>
        protected async Task<TModel> ConvertEntityToModel<TModel>(TEntity entity)
        {
            var readInterface = typeof(IReadableWithModel<,>).MakeGenericType(typeof(TEntity), typeof(TModel));
            var method = readInterface.GetMethod(nameof(IReadableWithModel<IEntity, IDto>.ConvertEntityToModel))
                ?? throw new Exception($"{GetType().Name} need implement {nameof(IReadableWithModel<IEntity, IDto>)}<{typeof(TEntity).Name}, {typeof(TModel).Name}>");

            var input = new object?[] { entity, null };
            if (method.Invoke(this, input) is not Task task)
                throw new Exception($"Error when convert {typeof(TEntity).Name} to {typeof(TModel).Name}");
            await task.WaitAsync(CancellationToken.None);

            if (input[1] == null)
                throw new Exception($"Error to convert {typeof(TEntity).Name} to {typeof(TModel).FullName}");
            return (TModel)input[1]!;
        }
    }
}
