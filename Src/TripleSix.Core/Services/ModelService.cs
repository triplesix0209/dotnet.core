using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TripleSix.Core.Dto;
using TripleSix.Core.Entities;
using TripleSix.Core.Exceptions;
using TripleSix.Core.Helpers;
using TripleSix.Core.Mappers;
using TripleSix.Core.Repositories;

namespace TripleSix.Core.Services
{
    public abstract class ModelService<TEntity> : BaseService<TEntity>,
        IModelService<TEntity>
        where TEntity : class, IModelEntity
    {
        protected ModelService(IModelRepository<TEntity> repo)
            : base(repo)
        {
            Repo = repo;
        }

        private IModelRepository<TEntity> Repo { get; }

        public virtual Task<string> GenerateCode(IIdentity identity, TEntity entity) => Task.FromResult<string>(null);

        public override sealed Task<TEntity> Create(IIdentity identity, TEntity entity)
        {
            return Create(identity, entity, true);
        }

        public virtual async Task<TEntity> Create(IIdentity identity, TEntity entity, bool autoGenerateCode)
        {
            if (autoGenerateCode && string.IsNullOrWhiteSpace(entity.Code))
                entity.Code = await GenerateCode(identity, entity);

            return await base.Create(identity, entity);
        }

        public new Task<TResult> CreateWithMapper<TResult>(IIdentity identity, IDataDto input)
            where TResult : class
        {
            return CreateWithMapper<TResult>(identity, input, true);
        }

        public async Task<TResult> CreateWithMapper<TResult>(IIdentity identity, IDataDto input, bool autoGenerateCode)
            where TResult : class
        {
            var data = await Create(identity, Mapper.MapData<IDataDto, TEntity>(input), autoGenerateCode);
            return Mapper.MapData<TEntity, TResult>(data);
        }

        public override sealed Task<IEnumerable<TEntity>> CreateBulk(IIdentity identity, IEnumerable<TEntity> entities)
        {
            return CreateBulk(identity, entities, true);
        }

        public virtual async Task<IEnumerable<TEntity>> CreateBulk(IIdentity identity, IEnumerable<TEntity> entities, bool autoGenerateCode)
        {
            if (autoGenerateCode)
            {
                foreach (var entity in entities)
                {
                    if (entity.Code.IsNotNullOrWhiteSpace()) continue;
                    entity.Code = await GenerateCode(identity, entity);
                }
            }

            await base.CreateBulk(identity, entities);
            return entities;
        }

        public new Task<IEnumerable<TResult>> CreateBulkWithMapper<TResult>(IIdentity identity, IEnumerable<IDataDto> input)
            where TResult : class
        {
            return CreateBulkWithMapper<TResult>(identity, input, true);
        }

        public async Task<IEnumerable<TResult>> CreateBulkWithMapper<TResult>(IIdentity identity, IEnumerable<IDataDto> input, bool autoGenerateCode)
            where TResult : class
        {
            var data = await CreateBulk(identity, Mapper.MapData<IEnumerable<IDataDto>, IEnumerable<TEntity>>(input), autoGenerateCode);
            return Mapper.MapData<IEnumerable<TEntity>, IEnumerable<TResult>>(data);
        }

        public override async Task Update(IIdentity identity, TEntity entity, Action<TEntity> @delegate)
        {
            if (typeof(TEntity).IsSubclassOfRawGeneric(typeof(ModelHierarchyEntity<>)))
            {
                var currentId = entity.Id;
                var currentEntity = entity.Clone() as TEntity;
                @delegate(currentEntity);

                var entityType = typeof(TEntity);
                Guid? parentId;
                do
                {
                    parentId = entityType.GetProperty(nameof(IModelHierarchyEntity<IModelEntity>.HierarchyParentId)).GetValue(currentEntity) as Guid?;
                    if (parentId.HasValue)
                    {
                        currentEntity = await Repo.Query.FirstOrDefaultAsync(x => x.Id == parentId);
                        if (currentEntity.Id == currentId) throw new BaseException(BaseExceptions.CyclicInheritance);
                    }
                }
                while (parentId.HasValue);
            }

            await base.Update(identity, entity, @delegate);
        }

        public async Task Update(IIdentity identity, Guid id, Action<TEntity> @delegate)
        {
            var entity = await GetFirstById(identity, id);
            await Update(identity, entity, @delegate);
        }

        public async Task Update(IIdentity identity, string code, Action<TEntity> @delegate)
        {
            var entity = await GetFirstByCode(identity, code);
            await Update(identity, entity, @delegate);
        }

        public async Task UpdateWithMapper(IIdentity identity, Guid id, IDataDto input)
        {
            var entity = await GetFirstById(identity, id);
            await UpdateWithMapper(identity, entity, input);
        }

        public async Task UpdateWithMapper(IIdentity identity, string code, IDataDto input)
        {
            var entity = await GetFirstByCode(identity, code);
            await UpdateWithMapper(identity, entity, input);
        }

        public async Task UpdateBulk(IIdentity identity, IEnumerable<Guid> listId, Action<TEntity> @delegate)
        {
            var entities = await GetListById(identity, listId);
            await UpdateBulk(identity, entities, @delegate);
        }

        public async Task UpdateBulk(IIdentity identity, IEnumerable<string> listCode, Action<TEntity> @delegate)
        {
            var entities = await GetListByCode(identity, listCode);
            await UpdateBulk(identity, entities, @delegate);
        }

        public async Task Delete(IIdentity identity, Guid id)
        {
            var entity = await GetFirstById(identity, id);
            await Delete(identity, entity);
        }

        public async Task Delete(IIdentity identity, string code)
        {
            var entity = await GetFirstByCode(identity, code);
            await Delete(identity, entity);
        }

        public async Task DeleteBulk(IIdentity identity, IEnumerable<Guid> listId)
        {
            var entities = await GetListById(identity, listId);
            await DeleteBulk(identity, entities);
        }

        public async Task DeleteBulk(IIdentity identity, IEnumerable<string> listCode)
        {
            var entities = await GetListByCode(identity, listCode);
            await DeleteBulk(identity, entities);
        }

        public virtual Task SetAsDelete(IIdentity identity, TEntity entity)
        {
            if (entity.IsDeleted) return Task.CompletedTask;
            return Update(identity, entity, e => e.IsDeleted = true);
        }

        public async Task SetAsDelete(IIdentity identity, Guid id)
        {
            var entity = await GetFirstById(identity, id);
            await SetAsDelete(identity, entity);
        }

        public async Task SetAsDelete(IIdentity identity, string code)
        {
            var entity = await GetFirstByCode(identity, code);
            await SetAsDelete(identity, entity);
        }

        public virtual async Task SetAsDeleteBulk(IIdentity identity, IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
                await SetAsDelete(identity, entity);
        }

        public async Task SetAsDeleteBulk(IIdentity identity, IEnumerable<Guid> listId)
        {
            var entities = await GetListById(identity, listId);
            await SetAsDeleteBulk(identity, entities);
        }

        public async Task SetAsDeleteBulk(IIdentity identity, IEnumerable<string> listCode)
        {
            var entities = await GetListByCode(identity, listCode);
            await SetAsDeleteBulk(identity, entities);
        }

        public async Task SetAsDeleteBulk(IIdentity identity, IQueryable<TEntity> query)
        {
            var entities = await query.ToListAsync<TEntity>(Mapper);
            await SetAsDeleteBulk(identity, entities);
        }

        public virtual Task Restore(IIdentity identity, TEntity entity)
        {
            if (!entity.IsDeleted) return Task.CompletedTask;
            return Update(identity, entity, e => e.IsDeleted = false);
        }

        public async Task Restore(IIdentity identity, Guid id)
        {
            var entity = await GetFirstById(identity, id);
            await Restore(identity, entity);
        }

        public async Task Restore(IIdentity identity, string code)
        {
            var entity = await GetFirstByCode(identity, code);
            await Restore(identity, entity);
        }

        public virtual async Task RestoreBulk(IIdentity identity, IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
                await Restore(identity, entity);
        }

        public async Task RestoreBulk(IIdentity identity, IEnumerable<Guid> listId)
        {
            var entities = await GetListById(identity, listId);
            await RestoreBulk(identity, entities);
        }

        public async Task RestoreBulk(IIdentity identity, IEnumerable<string> listCode)
        {
            var entities = await GetListByCode(identity, listCode);
            await RestoreBulk(identity, entities);
        }

        public async Task RestoreBulk(IIdentity identity, IQueryable<TEntity> query)
        {
            var entities = await query.ToListAsync<TEntity>(Mapper);
            await RestoreBulk(identity, entities);
        }

        public Task<bool> Any(IIdentity identity, bool includeDeleted = true)
        {
            var query = Repo.Query;
            if (!includeDeleted)
                query = query.WhereNotDeleted();

            return query.AnyAsync();
        }

        public Task<bool> AnyId(IIdentity identity, Guid id, bool includeDeleted = true)
        {
            var query = Repo.Query
                .Where(x => x.Id == id);
            if (!includeDeleted)
                query = query.WhereNotDeleted();

            return query.AnyAsync();
        }

        public Task<bool> AnyCode(IIdentity identity, string code, bool includeDeleted = true)
        {
            if (code.IsNullOrWhiteSpace())
                throw new ArgumentException("cannot be null or empty", nameof(code));

            var query = Repo.Query
                .Where(x => x.Code == code);
            if (!includeDeleted)
                query = query.WhereNotDeleted();

            return query.AnyAsync();
        }

        public Task<long> Count(IIdentity identity, bool includeDeleted = true)
        {
            var query = Repo.Query;
            if (!includeDeleted)
                query = query.WhereNotDeleted();

            return query.LongCountAsync();
        }

        public Task<TEntity> GetFirstById(IIdentity identity, Guid id, bool includeDeleted = true)
        {
            var query = Repo.Query
                .Where(x => x.Id == id);
            if (!includeDeleted)
                query = query.WhereNotDeleted();

            return query.FirstAsync<TEntity>(Mapper);
        }

        public Task<TResult> GetFirstById<TResult>(IIdentity identity, Guid id, bool includeDeleted = true)
            where TResult : class
        {
            var query = Repo.Query
                .Where(x => x.Id == id);
            if (!includeDeleted)
                query = query.WhereNotDeleted();

            return query.FirstAsync<TResult>(Mapper);
        }

        public async Task<TResult> GetFirstByIdWithModel<TResult>(IIdentity identity, Guid id, bool includeDeleted = true)
            where TResult : class, IDto
        {
            var query = Repo.Query
                .Where(x => x.Id == id);
            if (!includeDeleted)
                query = query.WhereNotDeleted();

            var entity = await query.FirstAsync<TEntity>(Mapper);
            return (await EntityToModel<TResult>(identity, entity)).First();
        }

        public Task<TEntity> GetFirstByCode(IIdentity identity, string code, bool includeDeleted = true)
        {
            if (code.IsNullOrWhiteSpace())
                throw new ArgumentException("cannot be null or empty", nameof(code));

            var query = Repo.Query
                .Where(x => x.Code == code);
            if (!includeDeleted)
                query = query.WhereNotDeleted();

            return query.FirstAsync<TEntity>(Mapper);
        }

        public Task<TResult> GetFirstByCode<TResult>(IIdentity identity, string code, bool includeDeleted = true)
            where TResult : class
        {
            if (code.IsNullOrWhiteSpace())
                throw new ArgumentException("cannot be null or empty", nameof(code));

            var query = Repo.Query
                .Where(x => x.Code == code);
            if (!includeDeleted)
                query = query.WhereNotDeleted();

            return query.FirstAsync<TResult>(Mapper);
        }

        public async Task<TResult> GetFirstByCodeWithModel<TResult>(IIdentity identity, string code, bool includeDeleted)
            where TResult : class, IDto
        {
            if (code.IsNullOrWhiteSpace())
                throw new ArgumentException("cannot be null or empty", nameof(code));

            var query = Repo.Query
                .Where(x => x.Code == code);
            if (!includeDeleted)
                query = query.WhereNotDeleted();

            var entity = await query.FirstAsync<TEntity>(Mapper);
            return (await EntityToModel<TResult>(identity, entity)).First();
        }

        public Task<TEntity> GetFirstOrDefaultById(IIdentity identity, Guid id, bool includeDeleted = true)
        {
            var query = Repo.Query
                .Where(x => x.Id == id);
            if (!includeDeleted)
                query = query.WhereNotDeleted();

            return query.FirstOrDefaultAsync<TEntity>(Mapper);
        }

        public Task<TResult> GetFirstOrDefaultById<TResult>(IIdentity identity, Guid id, bool includeDeleted = true)
            where TResult : class
        {
            var query = Repo.Query
                .Where(x => x.Id == id);
            if (!includeDeleted)
                query = query.WhereNotDeleted();

            return query.FirstOrDefaultAsync<TResult>(Mapper);
        }

        public async Task<TResult> GetFirstOrDefaultByIdWithModel<TResult>(IIdentity identity, Guid id, bool includeDeleted)
            where TResult : class, IDto
        {
            var query = Repo.Query
                .Where(x => x.Id == id);
            if (!includeDeleted)
                query = query.WhereNotDeleted();

            var entity = await query.FirstOrDefaultAsync<TEntity>(Mapper);
            return (await EntityToModel<TResult>(identity, entity)).First();
        }

        public Task<TEntity> GetFirstOrDefaultByCode(IIdentity identity, string code, bool includeDeleted = true)
        {
            if (code.IsNullOrWhiteSpace())
                throw new ArgumentException("cannot be null or empty", nameof(code));

            var query = Repo.Query
                .Where(x => x.Code == code);
            if (!includeDeleted)
                query = query.WhereNotDeleted();

            return query.FirstOrDefaultAsync<TEntity>(Mapper);
        }

        public Task<TResult> GetFirstOrDefaultByCode<TResult>(IIdentity identity, string code, bool includeDeleted = true)
            where TResult : class
        {
            if (code.IsNullOrWhiteSpace())
                throw new ArgumentException("cannot be null or empty", nameof(code));

            var query = Repo.Query
                .Where(x => x.Code == code);
            if (!includeDeleted)
                query = query.WhereNotDeleted();

            return query.FirstOrDefaultAsync<TResult>(Mapper);
        }

        public async Task<TResult> GetFirstOrDefaultByCodeWithModel<TResult>(IIdentity identity, string code, bool includeDeleted)
            where TResult : class, IDto
        {
            if (code.IsNullOrWhiteSpace())
                throw new ArgumentException("cannot be null or empty", nameof(code));

            var query = Repo.Query
                .Where(x => x.Code == code);
            if (!includeDeleted)
                query = query.WhereNotDeleted();

            var entity = await query.FirstOrDefaultAsync<TEntity>(Mapper);
            return (await EntityToModel<TResult>(identity, entity)).First();
        }

        public Task<TEntity[]> GetList(IIdentity identity, bool includeDeleted = true)
        {
            var query = Repo.Query;
            if (!includeDeleted)
                query = query.WhereNotDeleted();

            return query.ToArrayAsync<TEntity>(Mapper);
        }

        public Task<TResult[]> GetList<TResult>(IIdentity identity, bool includeDeleted = true)
            where TResult : class
        {
            var query = Repo.Query;
            if (!includeDeleted)
                query = query.WhereNotDeleted();

            return query.ToArrayAsync<TResult>(Mapper);
        }

        public async Task<TResult[]> GetListWithModel<TResult>(IIdentity identity, bool includeDeleted = true)
            where TResult : class, IDto
        {
            var query = Repo.Query;
            if (!includeDeleted)
                query = query.WhereNotDeleted();

            var entities = await query.ToArrayAsync<TEntity>(Mapper);
            return await EntityToModel<TResult>(identity, entities);
        }

        public async Task<TEntity[]> GetListByFilter(IIdentity identity, IFilterDto filter, bool includeDeleted = true)
        {
            var query = await Repo.BuildQueryOfFilter(filter, filter.GetType());
            if (!includeDeleted)
                query = query.WhereNotDeleted();

            return await query.ToArrayAsync<TEntity>(Mapper);
        }

        public async Task<TResult[]> GetListByFilter<TResult>(IIdentity identity, IFilterDto filter, bool includeDeleted = true)
            where TResult : class
        {
            var query = await Repo.BuildQueryOfFilter(filter, filter.GetType());
            if (!includeDeleted)
                query = query.WhereNotDeleted();

            return await query.ToArrayAsync<TResult>(Mapper);
        }

        public async Task<TResult[]> GetListByFilterWithModel<TResult>(IIdentity identity, IFilterDto filter)
            where TResult : class, IDto
        {
            var query = await Repo.BuildQueryOfFilter(filter, filter.GetType());
            var entities = await query.ToArrayAsync<TEntity>(Mapper);
            return await EntityToModel<TResult>(identity, entities);
        }

        public Task<TEntity[]> GetListById(IIdentity identity, IEnumerable<Guid> listId, bool includeDeleted = true)
        {
            var query = Repo.Query
                .Where(x => listId.Contains(x.Id));
            if (!includeDeleted)
                query = query.WhereNotDeleted();

            return query.ToArrayAsync<TEntity>(Mapper);
        }

        public Task<TResult[]> GetListById<TResult>(IIdentity identity, IEnumerable<Guid> listId, bool includeDeleted = true)
            where TResult : class
        {
            var query = Repo.Query
                .Where(x => listId.Contains(x.Id));
            if (!includeDeleted)
                query = query.WhereNotDeleted();

            return query.ToArrayAsync<TResult>(Mapper);
        }

        public async Task<TResult[]> GetListByIdWithModel<TResult>(IIdentity identity, IEnumerable<Guid> listId, bool includeDeleted)
            where TResult : class, IDto
        {
            var query = Repo.Query
                .Where(x => listId.Contains(x.Id));
            if (!includeDeleted)
                query = query.WhereNotDeleted();

            var entities = await query.ToArrayAsync<TEntity>(Mapper);
            return await EntityToModel<TResult>(identity, entities);
        }

        public Task<TEntity[]> GetListByCode(IIdentity identity, IEnumerable<string> listCode, bool includeDeleted = true)
        {
            if (listCode.Any(x => x.IsNullOrWhiteSpace()))
                throw new ArgumentException("cannot contain item that is null or empty", nameof(listCode));

            var query = Repo.Query
                .Where(x => listCode.Contains(x.Code));
            if (!includeDeleted)
                query = query.WhereNotDeleted();

            return query.ToArrayAsync<TEntity>(Mapper);
        }

        public Task<TResult[]> GetListByCode<TResult>(IIdentity identity, IEnumerable<string> listCode, bool includeDeleted = true)
            where TResult : class
        {
            if (listCode.Any(x => x.IsNullOrWhiteSpace()))
                throw new ArgumentException("cannot contain item that is null or empty", nameof(listCode));

            var query = Repo.Query
                .Where(x => listCode.Contains(x.Code));
            if (!includeDeleted)
                query = query.WhereNotDeleted();

            return query.ToArrayAsync<TResult>(Mapper);
        }

        public async Task<TResult[]> GetListByCodeWithModel<TResult>(IIdentity identity, IEnumerable<string> listCode, bool includeDeleted)
            where TResult : class, IDto
        {
            if (listCode.Any(x => x.IsNullOrWhiteSpace()))
                throw new ArgumentException("cannot contain item that is null or empty", nameof(listCode));

            var query = Repo.Query
               .Where(x => listCode.Contains(x.Code));
            if (!includeDeleted)
                query = query.WhereNotDeleted();

            var entities = await query.ToArrayAsync<TEntity>(Mapper);
            return await EntityToModel<TResult>(identity, entities);
        }

        public Task<IPaging<TEntity>> GetPage(IIdentity identity, int page, int size = 10, bool includeDeleted = true)
        {
            var query = Repo.Query;
            if (!includeDeleted)
                query = query.WhereNotDeleted();

            return query.ToPagingAsync<TEntity>(Mapper, page, size);
        }

        public Task<IPaging<TResult>> GetPage<TResult>(IIdentity identity, int page, int size = 10, bool includeDeleted = true)
            where TResult : class
        {
            var query = Repo.Query;
            if (!includeDeleted)
                query = query.WhereNotDeleted();

            return query.ToPagingAsync<TResult>(Mapper, page, size);
        }

        public async Task<IPaging<TResult>> GetPageWithModel<TResult>(IIdentity identity, int page, int size, bool includeDeleted)
            where TResult : class, IDto
        {
            var query = Repo.Query;
            if (!includeDeleted)
                query = query.WhereNotDeleted();

            var data = await query.ToPagingAsync<TEntity>(Mapper, page, size);
            return new Paging<TResult>
            {
                Page = data.Page,
                Size = data.Size,
                Total = data.Total,
                Items = await EntityToModel<TResult>(identity, data.Items),
            };
        }

        public async Task<IPaging<TResult>> GetPageByFilterWithModel<TResult>(IIdentity identity, IPagingFilterDto filter)
            where TResult : class, IDto
        {
            var query = await Repo.BuildQueryOfFilter(filter, filter.GetType());
            var data = await query.ToPagingAsync<TEntity>(Mapper, filter.Page, filter.Size);
            return new Paging<TResult>
            {
                Page = data.Page,
                Size = data.Size,
                Total = data.Total,
                Items = await EntityToModel<TResult>(identity, data.Items),
            };
        }

        protected async Task<TModel[]> EntityToModel<TModel>(IIdentity identity, params TEntity[] entities)
            where TModel : class, IDto
        {
            var serviceType = GetType();
            var readInterface = typeof(IReadableWithModel<,>).MakeGenericType(typeof(TEntity), typeof(TModel));
            if (!readInterface.IsAssignableFrom(serviceType))
                throw new Exception($"{serviceType.Name} need implement IReadableWithModel<{typeof(TEntity).Name},{typeof(TModel).Name}> interface");

            var convertMethod = serviceType.GetMethods().First(x =>
                x.Name == nameof(IReadableWithModel<TEntity, IDto>.ConvertEntityToModel) &&
                x.GetParameters().Length == 3 &&
                x.GetParameters()[2].ParameterType == typeof(TModel));

            var result = new List<Task<TModel>>();
            foreach (var entity in entities)
            {
                if (entity == null)
                    result.Add(Task.FromResult<TModel>(null));
                else
                    result.Add((Task<TModel>)convertMethod.Invoke(this, new object[] { identity, entity, null }));
            }

            await Task.WhenAll(result);
            return result.Select(x => x.Result).ToArray();
        }
    }
}
