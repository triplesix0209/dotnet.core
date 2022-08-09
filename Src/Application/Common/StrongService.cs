namespace Sample.Application.Common
{
    public abstract class StrongService<TEntity> : StrongService<TEntity, IApplicationDbContext>
        where TEntity : class, IStrongEntity
    {
    }
}
