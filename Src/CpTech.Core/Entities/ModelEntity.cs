using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CpTech.Core.Entities
{
    public abstract class ModelEntity<TEntity> : BaseEntity<TEntity>,
        IModelEntity
        where TEntity : class, IModelEntity, new()
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Guid Id { get; set; }

        [MaxLength(32)]
        public virtual string Code { get; set; }

        [Required]
        public virtual bool IsDeleted { get; set; }

        public virtual DateTime? CreateDatetime { get; set; }

        public virtual DateTime? UpdateDatetime { get; set; }

        public Guid? CreatorId { get; set; }

        public Guid? UpdaterId { get; set; }

        protected override void ModelConfigure(EntityTypeBuilder<TEntity> builder)
        {
            base.ModelConfigure(builder);

            builder.HasIndex(x => x.Code).IsUnique();
            builder.HasIndex(x => x.IsDeleted);
            builder.HasIndex(x => x.CreateDatetime);
            builder.HasIndex(x => x.UpdateDatetime);
            builder.HasIndex(x => x.CreatorId);
            builder.HasIndex(x => x.UpdaterId);
        }
    }
}