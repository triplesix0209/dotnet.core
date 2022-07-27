namespace Sample.Domain.Entities
{
    public class AccountAuth : StrongEntity<AccountAuth>
    {
        [MaxLength(100)]
        public string? Username { get; set; }

        [MaxLength(100)]
        public string? HashPasswordKey { get; set; }

        [MaxLength(128)]
        public string? HashedPassword { get; set; }

        public Guid AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        public virtual Account? Account { get; set; }

        public override void Configure(EntityTypeBuilder<AccountAuth> builder)
        {
            base.Configure(builder);

            builder.HasIndex(x => x.Username).IsUnique();
        }
    }
}
