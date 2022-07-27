using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TripleSix.Core.Entities
{
    public abstract class HierarchyEntity<TEntity> : StrongEntity<TEntity>,
        IHierarchyEntity<TEntity>
        where TEntity : class, IHierarchyEntity<TEntity>
    {
        /// <inheritdoc/>
        public virtual Guid? ParentId { get; set; }

        /// <inheritdoc/>
        public virtual int HierarchyLevel { get; set; }

        /// <inheritdoc/>
        public virtual TEntity? Parent { get; set; }

        /// <inheritdoc/>
        public virtual List<TEntity>? Childs { get; set; }

        /// <inheritdoc/>
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            base.Configure(builder);

            builder.HasOne(x => x.Parent)
                .WithMany(x => x.Childs)
                .HasForeignKey(x => x.ParentId);
        }
    }
}
