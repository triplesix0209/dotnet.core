using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TripleSix.Core.Entities
{
    /// <summary>
    /// Entity độc lập.
    /// </summary>
    /// <typeparam name="TEntity">Kiểu Entity, khai báo lại để sử dụng cho IEntityTypeConfiguration.</typeparam>
    public abstract class StrongEntity<TEntity> : BaseEntity<TEntity>,
        IStrongEntity
        where TEntity : class, IStrongEntity
    {
        /// <inheritdoc/>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Guid Id { get; set; }

        /// <inheritdoc/>
        public virtual DateTime? DeleteAt { get; set; }

        /// <inheritdoc/>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual DateTime? CreateAt { get; set; }

        /// <inheritdoc/>
        public virtual Guid? CreatorId { get; set; }

        /// <inheritdoc/>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public virtual DateTime? UpdateAt { get; set; }

        /// <inheritdoc/>
        public virtual Guid? UpdatorId { get; set; }

        /// <inheritdoc/>
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasIndex(x => x.DeleteAt);
            builder.HasIndex(x => x.CreateAt);
            builder.HasIndex(x => x.CreatorId);
            builder.HasIndex(x => x.UpdateAt);
            builder.HasIndex(x => x.UpdatorId);
        }
    }
}
