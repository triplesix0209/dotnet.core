namespace Sample.Domain.Entities
{
    public class Setting : BaseEntity<Setting>
    {
        [Key]
        [MaxLength(100)]
        public string Code { get; set; }

        public string? Value { get; set; }

        public string? Description { get; set; }

        public override void Configure(EntityTypeBuilder<Setting> builder)
        {
            base.Configure(builder);

            builder.HasIndex(x => x.Code).IsUnique();
        }
    }
}
