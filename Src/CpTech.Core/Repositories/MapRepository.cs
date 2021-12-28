using CpTech.Core.Dto;
using CpTech.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CpTech.Core.Repositories
{
    public abstract class MapRepository<TEntity> : BaseRepository<TEntity>,
        IMapRepository<TEntity>
        where TEntity : class, IMapEntity
    {
        protected MapRepository(DbContext dataContext)
            : base(dataContext)
        {
        }

        public override void Create(IIdentity identity, params TEntity[] entities)
        {
            if (identity != null)
            {
                foreach (var entity in entities)
                {
                    entity.CreatorId = identity.UserId;
                }
            }

            base.Create(identity, entities);
        }
    }
}