using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TripleSix.Core.Entities.Interfaces;

namespace TripleSix.Core.Entities
{
    /// <summary>
    /// Entity độc lập.
    /// </summary>
    /// <typeparam name="TEntity">Kiểu Entity, khai báo lại để sử dụng cho IEntityTypeConfiguration.</typeparam>
    public abstract class StrongEntity<TEntity> : BaseEntity<TEntity>, IStrongEntity
        where TEntity : class, IStrongEntity
    {
        /// <inheritdoc/>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <inheritdoc/>
        public bool IsDeleted { get; set; }

        /// <inheritdoc/>
        public DateTime? CreateDateTime { get; set; }

        /// <inheritdoc/>
        public Guid? CreatorId { get; set; }

        /// <inheritdoc/>
        public DateTime? UpdateDateTime { get; set; }

        /// <inheritdoc/>
        public Guid? UpdatorId { get; set; }

        /// <inheritdoc/>
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasIndex(x => x.IsDeleted);
            builder.HasIndex(x => x.CreateDateTime);
            builder.HasIndex(x => x.CreatorId);
            builder.HasIndex(x => x.UpdateDateTime);
            builder.HasIndex(x => x.UpdatorId);
        }
    }
}
