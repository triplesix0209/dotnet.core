using Sample.Domain.Persistences;
using TripleSix.Core.AutoAdmin;

namespace Sample.Infrastructure.Persistences
{
    public partial class ApplicationDbContext :
        IApplicationDbContext,
        IObjectLogDbContext
    {
        public DbSet<Account> Account { get; set; }

        public DbSet<AccountAuth> AccountAuth { get; set; }

        public DbSet<AccountSession> AccountSession { get; set; }

        public DbSet<ObjectLog> ObjectLog { get; set; }

        public DbSet<ObjectLogField> ObjectLogField { get; set; }

        public DbSet<Setting> Setting { get; set; }
    }
}
