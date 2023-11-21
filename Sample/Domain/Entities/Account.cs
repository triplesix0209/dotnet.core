namespace Sample.Domain.Entities
{
    [Comment("Tài khoản")]
    public class Account : StrongEntity<Account>
    {
        [Comment("Mã số")]
        [MaxLength(100)]
        public string Code { get; set; }

        [Comment("Tên gọi")]
        [MaxLength(200)]
        public string Name { get; set; }

        [Comment("Id chi nhánh")]
        public Guid SiteId { get; set; }

        [ForeignKey(nameof(SiteId))]
        public virtual Site Site { get; set; }

        /// <inheritdoc/>
        public override void Configure(EntityTypeBuilder<Account> builder)
        {
            base.Configure(builder);

            builder.HasIndex(x => x.Code).IsUnique();
            builder.HasIndex(x => x.Name);
        }
    }
}
