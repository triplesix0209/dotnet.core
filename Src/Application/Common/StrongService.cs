using TripleSix.Core.Entities;
using TripleSix.Core.Services;

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
