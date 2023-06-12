using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TripleSix.Core.Entities
{
    public abstract class HierarchyEntity<TEntity> : StrongEntity<TEntity>,
        IHierarchyEntity<TEntity>
        where TEntity : class, IHierarchyEntity<TEntity>
    {
        [Comment("Id mục cha")]
        public virtual Guid? ParentId { get; set; }

        [Comment("Số thứ tự cấp")]
        public virtual int HierarchyLevel { get; set; }

        [Comment("Mục cha")]
        public virtual TEntity? Parent { get; set; }

        public virtual List<TEntity>? Childs { get; set; }

        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            base.Configure(builder);

            builder.HasOne(x => x.Parent)
                .WithMany(x => x.Childs)
                .HasForeignKey(x => x.ParentId);
        }
    }
}
