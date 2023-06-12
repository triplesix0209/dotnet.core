namespace Sample.Domain.Entities
{
    public class Setting : StrongEntity<Setting>
    {
        [Comment("Mã")]
        public string Code { get; set; }

        [Comment("Giá trị")]
        public string? Value { get; set; }

        [Comment("Mô tả")]
        public string? Description { get; set; }

        public override void Configure(EntityTypeBuilder<Setting> builder)
        {
            base.Configure(builder);

            builder.HasIndex(x => x.Code).IsUnique();
        }
    }
}
