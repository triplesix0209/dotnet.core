using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TripleSix.CoreOld.Entities;

namespace Sample.Data.Entities
{
    public class SettingEntity : ModelEntity<SettingEntity>
    {
        [Required]
        [MaxLength(100)]
        public override string Code { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }

        protected override void ModelConfigure(EntityTypeBuilder<SettingEntity> builder)
        {
            base.ModelConfigure(builder);

            builder.HasIndex(x => x.Description);
        }
    }
}
