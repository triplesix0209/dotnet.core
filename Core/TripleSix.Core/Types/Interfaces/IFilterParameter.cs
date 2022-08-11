using TripleSix.Core.Entities;

namespace TripleSix.Core.Types
{
    public interface IFilterParameter
    {
        IQueryable<TEntity> ToQueryable<TEntity>(IQueryable<TEntity> query, string propertyName)
            where TEntity : class, IEntity;
    }
}
