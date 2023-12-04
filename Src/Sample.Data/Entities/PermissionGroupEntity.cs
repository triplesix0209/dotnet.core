using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TripleSix.CoreOld.Entities;

namespace Sample.Data.Entities
{
    public class PermissionGroupEntity : ModelHierarchyEntity<PermissionGroupEntity>
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public virtual IList<AccountEntity> Accounts { get; set; }

        public virtual IList<PermissionValueEntity> PermissionValues { get; set; }

        protected override void ModelConfigure(EntityTypeBuilder<PermissionGroupEntity> builder)
        {
            base.ModelConfigure(builder);

            builder.HasIndex(x => x.Name);
        }
    }
}
