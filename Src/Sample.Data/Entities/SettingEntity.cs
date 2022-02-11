using System.ComponentModel.DataAnnotations;
using TripleSix.Core.Entities;

namespace Sample.Data.Entities
{
    public class SettingEntity : ModelEntity<SettingEntity>
    {
        [Required]
        [MaxLength(100)]
        public override string Code { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }
    }
}
