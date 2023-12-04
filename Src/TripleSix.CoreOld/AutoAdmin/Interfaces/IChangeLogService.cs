using System;
using System.Threading.Tasks;
using TripleSix.CoreOld.Dto;
using TripleSix.CoreOld.Entities;
using TripleSix.CoreOld.Services;

namespace TripleSix.CoreOld.AutoAdmin
{
    public interface IChangeLogService<TEntity> : IModelService<TEntity>
        where TEntity : class, IModelEntity
    {
        Task<string> SerializeEntity(IIdentity identity, TEntity entity);

        Task<IPaging<ObjectLogDto>> GetPageChangeLog(IIdentity identity, Guid id, int page, int size = 10);

        Task<ObjectLogDto> GetChangeLog(IIdentity identity, Guid objectLogId);

        Task WriteChangeLog(IIdentity identity, Guid id, string beforeData = null, string note = null);
    }
}
