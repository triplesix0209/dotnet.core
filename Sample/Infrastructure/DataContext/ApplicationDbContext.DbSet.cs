using Sample.Domain.DataContext;

namespace Sample.Infrastructure.DataContext
{
    public partial class ApplicationDbContext : IApplicationDbContext
    {
        public DbSet<Setting> Setting { get; set; }

        public DbSet<Account> Account { get; set; }
    }
}
