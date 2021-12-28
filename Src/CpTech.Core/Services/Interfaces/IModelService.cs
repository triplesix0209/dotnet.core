using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CpTech.Core.Dto;
using CpTech.Core.Entities;

namespace CpTech.Core.Services
{
    public interface IModelService<TEntity> : IService<TEntity>
        where TEntity : class, IModelEntity
    {
        Task<string> GenerateCode(IIdentity identity, TEntity entity);

        Task<TEntity> Create(IIdentity identity, TEntity entity, bool autoGenerateCode);

        Task<TResult> CreateWithMapper<TResult>(IIdentity identity, IDataDto input, bool autoGenerateCode)
            where TResult : class;

        Task<IEnumerable<TEntity>> CreateBulk(IIdentity identity, IEnumerable<TEntity> entities, bool autoGenerateCode);

        Task<IEnumerable<TResult>> CreateBulkWithMapper<TResult>(IIdentity identity, IEnumerable<IDataDto> input, bool autoGenerateCode)
            where TResult : class;

        Task Update(IIdentity identity, Guid id, Action<TEntity> @delegate);

        Task Update(IIdentity identity, string code, Action<TEntity> @delegate);

        Task UpdateWithMapper(IIdentity identity, Guid id, IDataDto input);

        Task UpdateWithMapper(IIdentity identity, string code, IDataDto input);

        Task UpdateBulk(IIdentity identity, IEnumerable<Guid> listId, Action<TEntity> @delegate);

        Task UpdateBulk(IIdentity identity, IEnumerable<string> listCode, Action<TEntity> @delegate);

        Task Delete(IIdentity identity, Guid id);

        Task Delete(IIdentity identity, string code);

        Task DeleteBulk(IIdentity identity, IEnumerable<Guid> listId);

        Task DeleteBulk(IIdentity identity, IEnumerable<string> listCode);

        Task SetAsDelete(IIdentity identity, TEntity entity);

        Task SetAsDelete(IIdentity identity, Guid id);

        Task SetAsDelete(IIdentity identity, string code);

        Task SetAsDeleteBulk(IIdentity identity, IEnumerable<TEntity> entities);

        Task SetAsDeleteBulk(IIdentity identity, IEnumerable<Guid> listId);

        Task SetAsDeleteBulk(IIdentity identity, IEnumerable<string> listCode);

        Task SetAsDeleteBulk(IIdentity identity, IQueryable<TEntity> query);

        Task Restore(IIdentity identity, TEntity entity);

        Task Restore(IIdentity identity, Guid id);

        Task Restore(IIdentity identity, string code);

        Task RestoreBulk(IIdentity identity, IEnumerable<TEntity> entities);

        Task RestoreBulk(IIdentity identity, IEnumerable<Guid> listId);

        Task RestoreBulk(IIdentity identity, IEnumerable<string> listCode);

        Task RestoreBulk(IIdentity identity, IQueryable<TEntity> query);

        Task<bool> Any(IIdentity identity, bool includeDeleted = true);

        Task<bool> AnyId(IIdentity identity, Guid id, bool includeDeleted = true);

        Task<bool> AnyCode(IIdentity identity, string code, bool includeDeleted = true);

        Task<long> Count(IIdentity identity, bool includeDeleted = true);

        Task<TEntity> GetFirstById(IIdentity identity, Guid id, bool includeDeleted = true);

        Task<TResult> GetFirstById<TResult>(IIdentity identity, Guid id, bool includeDeleted = true)
            where TResult : class;

        Task<TResult> GetFirstByIdWithModel<TResult>(IIdentity identity, Guid id, bool includeDeleted = true)
            where TResult : class, IDto;

        Task<TEntity> GetFirstByCode(IIdentity identity, string code, bool includeDeleted = true);

        Task<TResult> GetFirstByCode<TResult>(IIdentity identity, string code, bool includeDeleted = true)
            where TResult : class;

        Task<TResult> GetFirstByCodeWithModel<TResult>(IIdentity identity, string code, bool includeDeleted = true)
            where TResult : class, IDto;

        Task<TEntity> GetFirstOrDefaultById(IIdentity identity, Guid id, bool includeDeleted = true);

        Task<TResult> GetFirstOrDefaultById<TResult>(IIdentity identity, Guid id, bool includeDeleted = true)
            where TResult : class;

        Task<TResult> GetFirstOrDefaultByIdWithModel<TResult>(IIdentity identity, Guid id, bool includeDeleted = true)
            where TResult : class, IDto;

        Task<TEntity> GetFirstOrDefaultByCode(IIdentity identity, string code, bool includeDeleted = true);

        Task<TResult> GetFirstOrDefaultByCode<TResult>(IIdentity identity, string code, bool includeDeleted = true)
            where TResult : class;

        Task<TResult> GetFirstOrDefaultByCodeWithModel<TResult>(IIdentity identity, string code, bool includeDeleted = true)
            where TResult : class, IDto;

        Task<TEntity[]> GetList(IIdentity identity, bool includeDeleted = true);

        Task<TResult[]> GetList<TResult>(IIdentity identity, bool includeDeleted = true)
            where TResult : class;

        Task<TResult[]> GetListWithModel<TResult>(IIdentity identity, bool includeDeleted = true)
            where TResult : class, IDto;

        Task<TResult[]> GetListByFilterWithModel<TResult>(IIdentity identity, IFilterDto filter)
            where TResult : class, IDto;

        Task<TEntity[]> GetListById(IIdentity identity, IEnumerable<Guid> listId, bool includeDeleted = true);

        Task<TResult[]> GetListById<TResult>(IIdentity identity, IEnumerable<Guid> listId, bool includeDeleted = true)
            where TResult : class;

        Task<TResult[]> GetListByIdWithModel<TResult>(IIdentity identity, IEnumerable<Guid> listId, bool includeDeleted = true)
            where TResult : class, IDto;

        Task<TEntity[]> GetListByCode(IIdentity identity, IEnumerable<string> listCode, bool includeDeleted = true);

        Task<TResult[]> GetListByCode<TResult>(IIdentity identity, IEnumerable<string> listCode, bool includeDeleted = true)
            where TResult : class;

        Task<TResult[]> GetListByCodeWithModel<TResult>(IIdentity identity, IEnumerable<string> listCode, bool includeDeleted = true)
            where TResult : class, IDto;

        Task<IPaging<TEntity>> GetPage(IIdentity identity, int page, int size = 10, bool includeDeleted = true);

        Task<IPaging<TResult>> GetPage<TResult>(IIdentity identity, int page, int size = 10, bool includeDeleted = true)
            where TResult : class;

        Task<IPaging<TResult>> GetPageWithModel<TResult>(IIdentity identity, int page, int size = 10, bool includeDeleted = true)
            where TResult : class, IDto;

        Task<IPaging<TResult>> GetPageByFilterWithModel<TResult>(IIdentity identity, IPagingFilterDto filter)
            where TResult : class, IDto;
    }
}
