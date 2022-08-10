using TripleSix.Core.Entities;
using TripleSix.Core.Types;

namespace TripleSix.Core.AutoAdmin
{
    public abstract class BaseAdminFilterDto<TEntity> : PagingInputDto, IQueryableDto<TEntity>
        where TEntity : class, IStrongEntity
    {
        public IQueryable<TEntity> ToQueryable(IQueryable<TEntity> query)
        {
            return query;
        }
    }
}
