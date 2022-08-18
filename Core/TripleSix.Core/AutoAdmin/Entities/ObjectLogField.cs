using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TripleSix.Core.Entities;

namespace TripleSix.Core.AutoAdmin
{
    public class ObjectLogField : BaseEntity<ObjectLogField>,
        IIdentifiableEntity,
        ICreateAuditableEntity
    {
        /// <inheritdoc/>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Guid Id { get; set; }

        /// <inheritdoc/>
        public virtual DateTime? CreateDateTime { get; set; }

        /// <inheritdoc/>
        public virtual Guid? CreatorId { get; set; }

        public virtual string FieldName { get; set; }

        public virtual string? OldValue { get; set; }

        public virtual string NewValue { get; set; }

        public virtual Guid LogId { get; set; }

        [ForeignKey(nameof(LogId))]
        public virtual ObjectLog Log { get; set; }

        public override void Configure(EntityTypeBuilder<ObjectLogField> builder)
        {
            base.Configure(builder);

            builder.HasIndex(x => x.CreateDateTime);
            builder.HasIndex(x => x.CreatorId);
        }
    }
}
