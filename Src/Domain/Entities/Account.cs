namespace Sample.Domain.Entities
{
    public class Account : StrongEntity<Account>
    {
        [Required]
        public string? Name { get; set; }

        public DateTime? BirthDate { get; set; }

        [Required]
        public string? Username { get; set; }

        public string? Password { get; set; }

        public override void Configure(EntityTypeBuilder<Account> builder)
        {
            base.Configure(builder);

            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.Username).IsUnique();
        }
    }
}
