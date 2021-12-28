using TripleSix.Core.Entities;

namespace TripleSix.Core.Repositories
{
    public interface IMapRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IMapEntity
    {
    }
}