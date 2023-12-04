using System.ComponentModel.DataAnnotations;

namespace Sample.Data.Entities
{
    public class TestEntity : ModelEntity<SettingEntity>
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        protected override void ModelConfigure(EntityTypeBuilder<SettingEntity> builder)
        {
            base.ModelConfigure(builder);
        }
    }
}
