namespace Sample.Domain.Entities
{
    public class Account : StrongEntity<Account>
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string? AvatarLink { get; set; }

        public AccountLevels AccessLevel { get; set; }

        public Guid? PermissionGroupId { get; set; }

        public virtual List<AccountAuth> Auths { get; set; }

        public virtual List<AccountSession> Sessions { get; set; }

        public override void Configure(EntityTypeBuilder<Account> builder)
        {
            base.Configure(builder);

            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.AccessLevel);
        }
    }
}
