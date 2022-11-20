using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TripleSix.Core.Entities;

namespace Sample.Data.Entities
{
    public class TestEntity : ModelEntity<SettingEntity>
    {
        [Required]
        [MaxLength(100)]
        public override string Code { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        protected override void ModelConfigure(EntityTypeBuilder<SettingEntity> builder)
        {
            base.ModelConfigure(builder);
        }
    }
}
