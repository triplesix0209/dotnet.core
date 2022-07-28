namespace Sample.Domain.Common
{
    public abstract class BaseQueryDto<TEntity> : BaseQueryModelDto<TEntity, IApplicationDbContext>
        where TEntity : class, IEntity
    {
    }
}
