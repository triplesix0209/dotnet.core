using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sample.Common.Enum;
using TripleSix.Core.Entities;

namespace Sample.Data.Entities
{
    public class PermissionValueEntity : BaseEntity<PermissionValueEntity>
    {
        [Required]
        [MaxLength(100)]
        public string PermissionCode { get; set; }

        public Guid PermissionGroupId { get; set; }

        public PermissionValues Value { get; set; }

        public bool ActualValue { get; set; }

        [ForeignKey(nameof(PermissionCode))]
        public virtual PermissionEntity Permission { get; set; }

        [ForeignKey(nameof(PermissionGroupId))]
        public virtual PermissionGroupEntity PermissionGroup { get; set; }

        protected override void ModelConfigure(EntityTypeBuilder<PermissionValueEntity> builder)
        {
            base.ModelConfigure(builder);

            builder.HasKey(x => new { x.PermissionCode, x.PermissionGroupId });
        }
    }
}
