using TripleSix.Core.Entities;
using TripleSix.Core.Repositories;
using TripleSix.Core.Services;

namespace Sample.Middle.Abstracts
{
    public abstract class CommonService<TEntity> : ModelService<TEntity>
        where TEntity : class, IModelEntity
    {
        public CommonService(IModelRepository<TEntity> repo)
           : base(repo)
        {
        }
    }
}
