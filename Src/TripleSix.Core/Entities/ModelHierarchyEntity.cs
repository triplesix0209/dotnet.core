using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TripleSix.Core.Entities
{
    public abstract class ModelHierarchyEntity<TEntity>
        : ModelEntity<TEntity>, IModelHierarchyEntity<TEntity>
        where TEntity : class, IModelHierarchyEntity<TEntity>, new()
    {
        public virtual Guid? HierarchyParentId { get; set; }

        public virtual TEntity HierarchyParent { get; set; }

        public virtual IList<TEntity> HierarchyChilds { get; set; }

        protected override void ModelConfigure(EntityTypeBuilder<TEntity> builder)
        {
            base.ModelConfigure(builder);

            builder.HasOne(src => src.HierarchyParent)
                .WithMany(desc => desc.HierarchyChilds)
                .HasForeignKey(x => x.HierarchyParentId);
        }
    }
}