using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TripleSix.Core.Entities;

namespace TripleSix.Core.AutoAdmin
{
    public class ObjectLogEntity : BaseEntity<ObjectLogEntity>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Guid Id { get; set; }

        public virtual DateTime? Datetime { get; set; }

        public Guid? ActorId { get; set; }

        [Required]
        public virtual string ObjectType { get; set; }

        public virtual Guid ObjectId { get; set; }

        public virtual string BeforeData { get; set; }

        [Required]
        public virtual string AfterData { get; set; }

        protected override void ModelConfigure(EntityTypeBuilder<ObjectLogEntity> builder)
        {
            base.ModelConfigure(builder);

            builder.HasIndex(x => x.Datetime);
            builder.HasIndex(x => x.ActorId);
            builder.HasIndex(x => new { x.ObjectType, x.ObjectId })
                .HasDatabaseName("IX_Object");
        }
    }
}