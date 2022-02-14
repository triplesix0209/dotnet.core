using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TripleSix.Core.Dto;
using TripleSix.Core.Entities;
using TripleSix.Core.Exceptions;
using TripleSix.Core.Helpers;
using TripleSix.Core.Mappers;
using TripleSix.Core.Repositories;

namespace TripleSix.Core.Services
{
    public abstract class BaseService<TEntity> : BaseService,
        IService<TEntity>
        where TEntity : class, IEntity
    {
        protected BaseService(IRepository<TEntity> repo)
        {
            Repo = repo;
        }

        private IRepository<TEntity> Repo { get; }

        public virtual async Task<TEntity> Create(IIdentity identity, TEntity entity)
        {
            Repo.Create(identity, entity);
            await Repo.SaveChanges();

            return entity;
        }

        public async Task<TResult> CreateWithMapper<TResult>(IIdentity identity, IDataDto input)
            where TResult : class
        {
            var data = await Create(identity, Mapper.MapData<IDataDto, TEntity>(input));
            return Mapper.MapData<TEntity, TResult>(data);
        }

        public virtual async Task<IEnumerable<TEntity>> CreateBulk(IIdentity identity, IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
                await Create(identity, entity);
            return entities;
        }

        public async Task<IEnumerable<TResult>> CreateBulkWithMapper<TResult>(IIdentity identity, IEnumerable<IDataDto> input)
            where TResult : class
        {
            var data = await CreateBulk(identity, Mapper.MapData<IEnumerable<IDataDto>, IEnumerable<TEntity>>(input));
            return Mapper.MapData<IEnumerable<TEntity>, IEnumerable<TResult>>(data);
        }

        public virtual async Task Update(IIdentity identity, TEntity entity, Action<TEntity> @delegate)
        {
            if (entity == null)
                throw new BaseException(BaseExceptions.ObjectNotFound, args: typeof(TEntity).GetDisplayName());

            @delegate(entity);

            Repo.Update(identity, entity);
            await Repo.SaveChanges();
        }

        public Task UpdateWithMapper(IIdentity identity, TEntity entity, IDataDto input)
        {
            return Update(identity, entity, entity => Mapper.MapUpdate(input, entity));
        }

        public virtual async Task UpdateBulk(IIdentity identity, IEnumerable<TEntity> entities, Action<TEntity> @delegate)
        {
            foreach (var entity in entities)
                await Update(identity, entity, @delegate);
        }

        public async Task UpdateBulk(IIdentity identity, IQueryable<TEntity> query, Action<TEntity> @delegate)
        {
            var entities = await query.ToArrayAsync<TEntity>(Mapper);
            await UpdateBulk(identity, entities, @delegate);
        }

        public virtual async Task Delete(IIdentity identity, TEntity entity)
        {
            Repo.Delete(entity);
            await Repo.SaveChanges();
        }

        public virtual async Task DeleteBulk(IIdentity identity, IEnumerable<TEntity> entities)
        {
            if (!entities.Any()) return;
            foreach (var entity in entities)
                await Delete(identity, entity);
        }

        public async Task DeleteBulk(IIdentity identity, IQueryable<TEntity> query)
        {
            var entities = await query.ToArrayAsync<TEntity>(Mapper);
            await DeleteBulk(identity, entities);
        }

        public Task<bool> Any(IIdentity identity)
        {
            return Repo.Query.AnyAsync();
        }

        public Task<bool> Any(IIdentity identity, IQueryable<TEntity> query)
        {
            return query.AnyAsync();
        }

        public async Task<bool> AnyByFilter(IIdentity identity, IFilterDto filter)
        {
            var query = await Repo.BuildQueryOfFilter(identity, filter, filter.GetType());
            return await query.AnyAsync();
        }

        public Task<long> Count(IIdentity identity)
        {
            return Repo.Query.LongCountAsync();
        }

        public Task<long> Count(IIdentity identity, IQueryable<TEntity> query)
        {
            return query.LongCountAsync();
        }

        public async Task<long> CountByFilter(IIdentity identity, IFilterDto filter)
        {
            var query = await Repo.BuildQueryOfFilter(identity, filter, filter.GetType());
            return await query.LongCountAsync();
        }

        public Task<TEntity> GetFirst(IIdentity identity, IQueryable<TEntity> query)
        {
            return query.FirstAsync<TEntity>(Mapper);
        }

        public Task<TResult> GetFirst<TResult>(IIdentity identity, IQueryable<TEntity> query)
            where TResult : class
        {
            return query.FirstAsync<TResult>(Mapper);
        }

        public async Task<TEntity> GetFirstByFilter(IIdentity identity, IFilterDto filter)
        {
            var query = await Repo.BuildQueryOfFilter(identity, filter, filter.GetType());
            return await query.FirstAsync<TEntity>(Mapper);
        }

        public async Task<TResult> GetFirstByFilter<TResult>(IIdentity identity, IFilterDto filter)
            where TResult : class
        {
            var query = await Repo.BuildQueryOfFilter(identity, filter, filter.GetType());
            return await query.FirstAsync<TResult>(Mapper);
        }

        public Task<TEntity> GetFirstOrDefault(IIdentity identity, IQueryable<TEntity> query)
        {
            return query.FirstOrDefaultAsync<TEntity>(Mapper);
        }

        public Task<TResult> GetFirstOrDefault<TResult>(IIdentity identity, IQueryable<TEntity> query)
            where TResult : class
        {
            return query.FirstOrDefaultAsync<TResult>(Mapper);
        }

        public async Task<TEntity> GetFirstOrDefaultByFilter(IIdentity identity, IFilterDto filter)
        {
            var query = await Repo.BuildQueryOfFilter(filter, filter.GetType());
            return await query.FirstOrDefaultAsync<TEntity>(Mapper);
        }

        public async Task<TResult> GetFirstOrDefaultByFilter<TResult>(IIdentity identity, IFilterDto filter)
            where TResult : class
        {
            var query = await Repo.BuildQueryOfFilter(filter, filter.GetType());
            return await query.FirstOrDefaultAsync<TResult>(Mapper);
        }

        public Task<TEntity[]> GetList(IIdentity identity)
        {
            return Repo.Query.ToArrayAsync<TEntity>(Mapper);
        }

        public Task<TResult[]> GetList<TResult>(IIdentity identity)
            where TResult : class
        {
            return Repo.Query.ToArrayAsync<TResult>(Mapper);
        }

        public Task<TEntity[]> GetList(IIdentity identity, IQueryable<TEntity> query)
        {
            return query.ToArrayAsync<TEntity>(Mapper);
        }

        public Task<TResult[]> GetList<TResult>(IIdentity identity, IQueryable<TEntity> query)
            where TResult : class
        {
            return query.ToArrayAsync<TResult>(Mapper);
        }

        public async Task<TEntity[]> GetListByFilter(IIdentity identity, IFilterDto filter)
        {
            var query = await Repo.BuildQueryOfFilter(filter, filter.GetType());
            return await query.ToArrayAsync<TEntity>(Mapper);
        }

        public async Task<TResult[]> GetListByFilter<TResult>(IIdentity identity, IFilterDto filter)
            where TResult : class
        {
            var query = await Repo.BuildQueryOfFilter(filter, filter.GetType());
            return await query.ToArrayAsync<TResult>(Mapper);
        }

        public Task<IPaging<TEntity>> GetPage(IIdentity identity, int page, int size = 10)
        {
            return Repo.Query.ToPagingAsync<TEntity>(Mapper, page, size);
        }

        public Task<IPaging<TResult>> GetPage<TResult>(IIdentity identity, int page, int size = 10)
            where TResult : class
        {
            return Repo.Query.ToPagingAsync<TResult>(Mapper, page, size);
        }

        public Task<IPaging<TEntity>> GetPage(IIdentity identity, IQueryable<TEntity> query, int page, int size = 10)
        {
            return query.ToPagingAsync<TEntity>(Mapper, page, size);
        }

        public Task<IPaging<TResult>> GetPage<TResult>(IIdentity identity, IQueryable<TEntity> query, int page, int size = 10)
            where TResult : class
        {
            return query.ToPagingAsync<TResult>(Mapper, page, size);
        }

        public async Task<IPaging<TEntity>> GetPageByFilter(IIdentity identity, IPagingFilterDto filter)
        {
            var query = await Repo.BuildQueryOfFilter(filter, filter.GetType());
            return await query.ToPagingAsync<TEntity>(Mapper, filter.Page, filter.Size);
        }

        public async Task<IPaging<TResult>> GetPageByFilter<TResult>(IIdentity identity, IPagingFilterDto filter)
            where TResult : class
        {
            var query = await Repo.BuildQueryOfFilter(filter, filter.GetType());
            return await query.ToPagingAsync<TResult>(Mapper, filter.Page, filter.Size);
        }

        protected Task<IDbContextTransaction> BeginTransaction() => Repo.BeginTransaction();

        protected Task SaveChanges() => Repo.SaveChanges();
    }
}