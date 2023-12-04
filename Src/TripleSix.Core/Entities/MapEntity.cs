using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TripleSix.Core.Entities
{
    public abstract class MapEntity<TEntity> : BaseEntity<TEntity>,
        IMapEntity
        where TEntity : class, IMapEntity, new()
    {
        public virtual DateTime? CreateDatetime { get; set; }

        public Guid? CreatorId { get; set; }

        protected override void ModelConfigure(EntityTypeBuilder<TEntity> builder)
        {
            base.ModelConfigure(builder);

            builder.HasIndex(x => x.CreateDatetime);
            builder.HasIndex(x => x.CreatorId);
        }
    }
}