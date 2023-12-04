using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sample.Data.Entities
{
    public class PermissionEntity : BaseEntity<PermissionEntity>
    {
        [Key]
        [MaxLength(100)]
        public string Code { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; }

        public virtual IList<PermissionValueEntity> PermissionValues { get; set; }

        protected override void ModelConfigure(EntityTypeBuilder<PermissionEntity> builder)
        {
            base.ModelConfigure(builder);

            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.CategoryName);
        }
    }
}
