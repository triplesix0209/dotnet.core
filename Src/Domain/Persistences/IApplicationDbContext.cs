using TripleSix.Core.Persistences;

namespace Sample.Domain.Persistences
{
    public interface IApplicationDbContext : IDbDataContext
    {
        DbSet<Account> Account { get; set; }

        DbSet<AccountAuth> AccountAuth { get; set; }

        DbSet<AccountSession> AccountSession { get; set; }

        DbSet<Setting> Setting { get; set; }
    }
}
