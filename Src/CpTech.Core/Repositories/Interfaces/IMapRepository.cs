using CpTech.Core.Entities;

namespace CpTech.Core.Repositories
{
    public interface IMapRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IMapEntity
    {
    }
}