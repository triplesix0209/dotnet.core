namespace Sample.Infrastructure.DataContext
{
    public partial class ApplicationDbContext : IApplicationDbContext
    {
        /// <inheritdoc/>
        public DbSet<Account> Account { get; set; }

        /// <inheritdoc/>
        public DbSet<Site> Site { get; set; }
    }
}
