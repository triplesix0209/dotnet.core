namespace Sample.Application.Common
{
    public interface IStrongService<TEntity> : IStrongService<TEntity, IApplicationDbContext>
        where TEntity : class, IStrongEntity
    {
    }
}
