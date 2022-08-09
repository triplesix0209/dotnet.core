namespace Sample.Application.Common
{
    public abstract class BaseService<TEntity> : BaseService<TEntity, IApplicationDbContext>
        where TEntity : class, IEntity
    {
    }
}
