using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CpTech.Core.Entities
{
    public abstract class MapEntity<TEntity> : BaseEntity<TEntity>,
        IMapEntity
        where TEntity : class, IMapEntity, new()
    {
        public virtual DateTime? CreateDatetime { get; set; }

        public Guid? CreatorId { get; set; }

        public abstract void SetIds(params Guid[] listIds);

        protected override void ModelConfigure(EntityTypeBuilder<TEntity> builder)
        {
            base.ModelConfigure(builder);

            builder.HasIndex(x => x.CreateDatetime);
            builder.HasIndex(x => x.CreatorId);
        }
    }
}