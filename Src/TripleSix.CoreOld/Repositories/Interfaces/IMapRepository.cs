using TripleSix.CoreOld.Entities;

namespace TripleSix.CoreOld.Repositories
{
    public interface IMapRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IMapEntity
    {
    }
}