using System.ComponentModel.DataAnnotations;

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
