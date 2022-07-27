namespace Sample.Domain.Entities
{
    public class AccountSession : StrongEntity<AccountSession>
    {
        [Required]
        public override string? Code { get; set; }

        public DateTime ExpiryDatetime { get; set; }

        public Guid AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        public virtual Account? Account { get; set; }

        public override void Configure(EntityTypeBuilder<AccountSession> builder)
        {
            base.Configure(builder);

            builder.HasIndex(x => x.ExpiryDatetime);
        }
    }
}
