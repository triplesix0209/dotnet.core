using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TripleSix.Core.Entities;

namespace TripleSix.AutoAdmin.Entities
{
    public class ObjectLogEntity : ModelEntity<ObjectLogEntity>
    {
        [Required]
        public virtual string ObjectType { get; set; }

        public virtual Guid ObjectId { get; set; }

        public virtual string BeforeData { get; set; }

        [Required]
        public virtual string AfterData { get; set; }

        protected override void ModelConfigure(EntityTypeBuilder<ObjectLogEntity> builder)
        {
            base.ModelConfigure(builder);

            builder.HasIndex(x => new { x.ObjectType, x.ObjectId })
                .HasDatabaseName("IX_Object");
        }
    }
}