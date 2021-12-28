using System.Collections.Generic;
using CpTech.Core.Dto;
using CpTech.Core.Entities;

namespace CpTech.Core.Repositories
{
    public interface IModelRepository<TEntity> : IRepository<TEntity>,
        IQueryBuilder<TEntity, ModelFilterDto>
        where TEntity : class, IModelEntity
    {
        void SetAsDeleted(IIdentity identity, params TEntity[] entities);

        void SetAsDeleted(params TEntity[] entities);

        void SetAsDeleted(IIdentity identity, IEnumerable<TEntity> entities);

        void SetAsDeleted(IEnumerable<TEntity> entities);

        void Restore(IIdentity identity, params TEntity[] entities);

        void Restore(params TEntity[] entities);

        void Restore(IIdentity identity, IEnumerable<TEntity> entities);

        void Restore(IEnumerable<TEntity> entities);
    }
}