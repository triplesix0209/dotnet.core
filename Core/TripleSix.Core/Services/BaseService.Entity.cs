#pragma warning disable SA1401 // Fields should be private

using Autofac;
using Microsoft.EntityFrameworkCore;
using TripleSix.Core.Entities;
using TripleSix.Core.Exceptions;
using TripleSix.Core.Helpers;
using TripleSix.Core.Persistences;
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
        public IQueryable<TEntity> Query => _db.Set<TEntity>();

        /// <inheritdoc/>
        public virtual async Task<TEntity> Create(TEntity entity, CancellationToken cancellationToken = default)
        {
            using var activity = StartTraceMethodActivity();

            await _db.Set<TEntity>().AddAsync(entity, cancellationToken);
            await _db.SaveChangesAsync(true, cancellationToken);
            return entity;
        }

        /// <inheritdoc/>
        public async Task<TResult> CreateWithMapper<TResult>(IDto input, CancellationToken cancellationToken = default)
            where TResult : class
        {
            var entity = Mapper.MapData<IDto, TEntity>(input);
            var result = await Create(entity, cancellationToken);
            return Mapper.MapData<TEntity, TResult>(result);
        }

        /// <inheritdoc/>
        public virtual async Task Update(TEntity entity, Action<TEntity> updateMethod, CancellationToken cancellationToken = default)
        {
            using var activity = StartTraceMethodActivity();

            updateMethod(entity);
            _db.Set<TEntity>().Update(entity);

            await _db.SaveChangesAsync(true, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task UpdateWithMapper(TEntity entity, IDto input, CancellationToken cancellationToken = default)
        {
            if (!input.IsAnyPropertyChanged()) return;

            await Update(
                entity,
                e => Mapper.MapUpdate(input, e),
                cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task HardDelete(TEntity entity, CancellationToken cancellationToken = default)
        {
            using var activity = StartTraceMethodActivity();

            _db.Set<TEntity>().Remove(entity);

            await _db.SaveChangesAsync(true, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<bool> Any(IQueryable<TEntity>? query = default, CancellationToken cancellationToken = default)
        {
            using var activity = StartTraceMethodActivity();
            if (query == null) query = Query;

            return await query.AnyAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<bool> AnyByQueryModel(IQueryableDto<TEntity> model, CancellationToken cancellationToken = default)
        {
            return await Any(model.ToQueryable(Query), cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<long> Count(IQueryable<TEntity>? query = default, CancellationToken cancellationToken = default)
        {
            using var activity = StartTraceMethodActivity();
            if (query == null) query = Query;

            return await query.LongCountAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<long> CountByQueryModel(IQueryableDto<TEntity> model, CancellationToken cancellationToken = default)
        {
            return await Count(model.ToQueryable(Query), cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<TResult?> GetFirstOrDefault<TResult>(IQueryable<TEntity>? query = default, CancellationToken cancellationToken = default)
            where TResult : class
        {
            using var activity = StartTraceMethodActivity();
            if (query == null) query = Query;

            if (!CanConvertEntityToModel<TResult>())
                return await query.FirstOrDefaultAsync<TResult>(Mapper, cancellationToken);

            var data = await query.FirstOrDefaultAsync(cancellationToken);
            if (data == null) return null;

            return await ConvertEntityToModel<TResult>(data, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<TEntity?> GetFirstOrDefault(IQueryable<TEntity>? query = default, CancellationToken cancellationToken = default)
        {
            return await GetFirstOrDefault<TEntity>(query, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<TResult?> GetFirstOrDefaultByQueryModel<TResult>(IQueryableDto<TEntity> model, CancellationToken cancellationToken = default)
            where TResult : class
        {
            return await GetFirstOrDefault<TResult>(model.ToQueryable(Query), cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<TEntity?> GetFirstOrDefaultByQueryModel(IQueryableDto<TEntity> model, CancellationToken cancellationToken = default)
        {
            return await GetFirstOrDefault(model.ToQueryable(Query), cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<TResult> GetFirst<TResult>(IQueryable<TEntity>? query = default, CancellationToken cancellationToken = default)
            where TResult : class
        {
            var data = await GetFirstOrDefault<TResult>(query, cancellationToken);
            if (data == null) throw new EntityNotFoundException(typeof(TEntity));

            return data;
        }

        /// <inheritdoc/>
        public async Task<TEntity> GetFirst(IQueryable<TEntity>? query = default, CancellationToken cancellationToken = default)
        {
            return await GetFirst<TEntity>(query, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<TResult> GetFirstByQueryModel<TResult>(IQueryableDto<TEntity> model, CancellationToken cancellationToken = default)
            where TResult : class
        {
            return await GetFirst<TResult>(model.ToQueryable(Query), cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<TEntity> GetFirstByQueryModel(IQueryableDto<TEntity> model, CancellationToken cancellationToken = default)
        {
            return await GetFirst(model.ToQueryable(Query), cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<List<TResult>> GetList<TResult>(IQueryable<TEntity>? query = default, CancellationToken cancellationToken = default)
            where TResult : class
        {
            using var activity = StartTraceMethodActivity();
            if (query == null) query = Query;

            if (!CanConvertEntityToModel<TResult>())
                return await query.ToListAsync<TResult>(Mapper, cancellationToken);

            var data = await query.ToListAsync(cancellationToken);
            return await ConvertEntityToModel<TResult>(data, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<List<TEntity>> GetList(IQueryable<TEntity>? query = default, CancellationToken cancellationToken = default)
        {
            return await GetList<TEntity>(query, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<List<TResult>> GetListByQueryModel<TResult>(IQueryableDto<TEntity> model, CancellationToken cancellationToken = default)
            where TResult : class
        {
            return await GetList<TResult>(model.ToQueryable(Query), cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<List<TEntity>> GetListByQueryModel(IQueryableDto<TEntity> model, CancellationToken cancellationToken = default)
        {
            return await GetList(model.ToQueryable(Query), cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IPaging<TResult>> GetPage<TResult>(IQueryable<TEntity>? query = default, int page = 1, int size = 10, CancellationToken cancellationToken = default)
            where TResult : class
        {
            using var activity = StartTraceMethodActivity();

            if (page <= 0) throw new ArgumentOutOfRangeException(nameof(page), "must be greater than 0");
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size), "must be greater than 0");

            var result = new Paging<TResult>(page, size);
            if (query == null) query = Query;

            result.Total = await query.LongCountAsync(cancellationToken);
            if (!CanConvertEntityToModel<TResult>())
            {
                result.Items = await query
                    .Skip((page - 1) * size)
                    .Take(size)
                    .ToListAsync<TResult>(Mapper, cancellationToken);
            }
            else
            {
                var data = await query
                    .Skip((page - 1) * size)
                    .Take(size)
                    .ToListAsync(cancellationToken);
                result.Items = await ConvertEntityToModel<TResult>(data, cancellationToken);
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<IPaging<TEntity>> GetPage(IQueryable<TEntity>? query = default, int page = 1, int size = 10, CancellationToken cancellationToken = default)
        {
            return await GetPage<TEntity>(query, page, size, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IPaging<TResult>> GetPageByQueryModel<TResult>(IQueryableDto<TEntity> model, int page = 1, int size = 10, CancellationToken cancellationToken = default)
            where TResult : class
        {
            return await GetPage<TResult>(model.ToQueryable(Query), page, size, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IPaging<TEntity>> GetPageByQueryModel(IQueryableDto<TEntity> model, int page = 1, int size = 10, CancellationToken cancellationToken = default)
        {
            return await GetPage(model.ToQueryable(Query), page, size, cancellationToken);
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
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Danh sách dữ liệu sau khi chuyển đổi.</returns>
        protected async Task<List<TModel>> ConvertEntityToModel<TModel>(List<TEntity> entities, CancellationToken cancellationToken = default)
        {
            var readInterface = typeof(IReadableWithModel<,>).MakeGenericType(typeof(TEntity), typeof(TModel));
            var method = readInterface.GetMethod(nameof(IReadableWithModel<IEntity, IDto>.ConvertEntityToModel));
            if (method == null)
                throw new Exception($"{GetType().Name} need implement {nameof(IReadableWithModel<IEntity, IDto>)}<{typeof(TEntity).Name}, {typeof(TModel).Name}>");

            var result = new List<TModel>();
            foreach (var entity in entities)
            {
                var input = new object?[] { entity, null, cancellationToken };
                if (method.Invoke(this, input) is not Task task)
                    throw new Exception($"Error when convert {typeof(TEntity).Name} to {typeof(TModel).FullName}");
                await task.WaitAsync(cancellationToken);

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
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Dữ liệu sau khi chuyển đổi.</returns>
        protected async Task<TModel> ConvertEntityToModel<TModel>(TEntity entity, CancellationToken cancellationToken = default)
        {
            var readInterface = typeof(IReadableWithModel<,>).MakeGenericType(typeof(TEntity), typeof(TModel));
            var method = readInterface.GetMethod(nameof(IReadableWithModel<IEntity, IDto>.ConvertEntityToModel));
            if (method == null)
                throw new Exception($"{GetType().Name} need implement {nameof(IReadableWithModel<IEntity, IDto>)}<{typeof(TEntity).Name}, {typeof(TModel).Name}>");

            var input = new object?[] { entity, null, cancellationToken };
            if (method.Invoke(this, input) is not Task task)
                throw new Exception($"Error when convert {typeof(TEntity).Name} to {typeof(TModel).Name}");
            await task.WaitAsync(cancellationToken);

            if (input[1] == null)
                throw new Exception($"Error to convert {typeof(TEntity).Name} to {typeof(TModel).FullName}");
            return (TModel)input[1]!;
        }
    }
}
