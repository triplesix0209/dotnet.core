using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TripleSix.Core.Entities;

namespace TripleSix.Core.AutoAdmin
{
    public class ObjectLog : BaseEntity<ObjectLog>,
        IIdentifiableEntity,
        ICreateAuditableEntity
    {
        /// <inheritdoc/>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Guid Id { get; set; }

        /// <inheritdoc/>
        public virtual DateTime? CreateAt { get; set; }

        /// <inheritdoc/>
        public virtual Guid? CreatorId { get; set; }

        /// <summary>
        /// Loại đối tượng.
        /// </summary>
        public virtual string ObjectType { get; set; }

        /// <summary>
        /// Id đối tượng.
        /// </summary>
        public virtual Guid ObjectId { get; set; }

        /// <summary>
        /// Ghi chú.
        /// </summary>
        public virtual string? Note { get; set; }

        public virtual List<ObjectLogField> Fields { get; set; }

        public override void Configure(EntityTypeBuilder<ObjectLog> builder)
        {
            base.Configure(builder);

            builder.HasIndex(x => x.CreateAt);
            builder.HasIndex(x => x.CreatorId);
            builder.HasIndex(x => x.ObjectType);
            builder.HasIndex(x => x.ObjectId);
        }
    }
}
