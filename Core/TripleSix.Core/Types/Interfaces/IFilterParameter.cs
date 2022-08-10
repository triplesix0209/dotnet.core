using TripleSix.Core.Entities;

namespace TripleSix.Core.Types
{
    public interface IFilterParameter
    {
        IQueryable<TEntity> ToQueryable<TEntity>(IQueryable<TEntity> query)
            where TEntity : class, IEntity;
    }
}
