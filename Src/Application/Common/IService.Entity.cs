namespace Sample.Application.Common
{
    public interface IService<TEntity> : IService<TEntity, IApplicationDbContext>
        where TEntity : class, IEntity
    {
    }
}
