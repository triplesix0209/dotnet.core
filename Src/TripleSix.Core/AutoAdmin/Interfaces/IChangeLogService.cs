using System;
using System.Threading.Tasks;
using TripleSix.Core.Dto;
using TripleSix.Core.Entities;
using TripleSix.Core.Services;

namespace TripleSix.Core.AutoAdmin
{
    public interface IChangeLogService<TEntity> : IModelService<TEntity>
        where TEntity : class, IModelEntity
    {
        Task<string> SerializeEntity(IIdentity identity, TEntity entity);

        Task<IPaging<ObjectLogDto>> GetPageChangeLog(IIdentity identity, Guid id, int page, int size = 10);

        Task<ObjectLogDto> GetChangeLog(IIdentity identity, Guid objectLogId);

        Task WriteChangeLog(IIdentity identity, Guid id, string beforeData = null);
    }
}
