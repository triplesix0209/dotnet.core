using TripleSix.Core.Entities;

namespace Sample.Application.Common
{
    public abstract class StrongService<TEntity> : StrongService<IApplicationDbContext, TEntity>
        where TEntity : class, IStrongEntity
    {
        protected StrongService(IApplicationDbContext db)
            : base(db)
        {
        }
    }
}
