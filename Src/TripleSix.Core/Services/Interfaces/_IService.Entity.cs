#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TripleSix.Core.Dto;
using TripleSix.Core.Entities;

namespace TripleSix.Core.Services
{
    public interface IService<TEntity> : IService
        where TEntity : class, IEntity
    {
        Task<TEntity> Create(IIdentity identity, TEntity entity);

        Task<TResult> CreateWithMapper<TResult>(IIdentity identity, IDataDto input)
            where TResult : class;

        Task<IEnumerable<TEntity>> CreateBulk(IIdentity identity, IEnumerable<TEntity> entities);

        Task<IEnumerable<TResult>> CreateBulkWithMapper<TResult>(IIdentity identity, IEnumerable<IDataDto> input)
            where TResult : class;

        Task Update(IIdentity identity, TEntity entity, Action<TEntity> @delegate);

        Task UpdateWithMapper(IIdentity identity, TEntity entity, IDataDto input);

        Task UpdateBulk(IIdentity identity, IEnumerable<TEntity> entities, Action<TEntity> @delegate);

        Task UpdateBulk(IIdentity identity, IQueryable<TEntity> query, Action<TEntity> @delegate);

        Task Delete(IIdentity identity, TEntity entity);

        Task DeleteBulk(IIdentity identity, IEnumerable<TEntity> entities);

        Task DeleteBulk(IIdentity identity, IQueryable<TEntity> query);

        Task<bool> Any(IIdentity identity);

        Task<bool> Any(IIdentity identity, IQueryable<TEntity> query);

        Task<bool> AnyByFilter(IIdentity identity, IFilterDto filter);

        Task<long> Count(IIdentity identity);

        Task<long> Count(IIdentity identity, IQueryable<TEntity> query);

        Task<long> CountByFilter(IIdentity identity, IFilterDto filter);

        Task<TEntity> GetFirst(IIdentity identity, IQueryable<TEntity> query);

        Task<TResult> GetFirst<TResult>(IIdentity identity, IQueryable<TEntity> query)
            where TResult : class;

        Task<TEntity> GetFirstByFilter(IIdentity identity, IFilterDto filter);

        Task<TResult> GetFirstByFilter<TResult>(IIdentity identity, IFilterDto filter)
            where TResult : class;

        Task<TEntity> GetFirstOrDefault(IIdentity identity, IQueryable<TEntity> query);

        Task<TResult> GetFirstOrDefault<TResult>(IIdentity identity, IQueryable<TEntity> query)
            where TResult : class;

        Task<TEntity> GetFirstOrDefaultByFilter(IIdentity identity, IFilterDto filter);

        Task<TResult> GetFirstOrDefaultByFilter<TResult>(IIdentity identity, IFilterDto filter)
            where TResult : class;

        Task<TEntity[]> GetList(IIdentity identity);

        Task<TResult[]> GetList<TResult>(IIdentity identity)
            where TResult : class;

        Task<TEntity[]> GetList(IIdentity identity, IQueryable<TEntity> query);

        Task<TResult[]> GetList<TResult>(IIdentity identity, IQueryable<TEntity> query)
            where TResult : class;

        Task<TEntity[]> GetListByFilter(IIdentity identity, IFilterDto filter);

        Task<TResult[]> GetListByFilter<TResult>(IIdentity identity, IFilterDto filter)
            where TResult : class;

        Task<IPaging<TEntity>> GetPage(IIdentity identity, int page, int size = 10);

        Task<IPaging<TResult>> GetPage<TResult>(IIdentity identity, int page, int size = 10)
            where TResult : class;

        Task<IPaging<TEntity>> GetPage(IIdentity identity, IQueryable<TEntity> query, int page, int size = 10);

        Task<IPaging<TResult>> GetPage<TResult>(IIdentity identity, IQueryable<TEntity> query, int page, int size = 10)
            where TResult : class;

        Task<IPaging<TEntity>> GetPageByFilter(IIdentity identity, IPagingFilterDto filter);

        Task<IPaging<TResult>> GetPageByFilter<TResult>(IIdentity identity, IPagingFilterDto filter)
            where TResult : class;
    }
}