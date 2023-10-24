namespace Sample.Infrastructure.DataContext
{
    public partial class ApplicationDbContext : IApplicationDbContext
    {
        public DbSet<Account> Account { get; set; }

        public DbSet<Site> Site { get; set; }
    }
}
