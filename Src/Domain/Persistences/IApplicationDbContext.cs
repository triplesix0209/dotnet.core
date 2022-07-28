using TripleSix.Core.Persistences;

namespace Sample.Domain.Persistences
{
    public interface IApplicationDbContext : IDbDataContext
    {
        DbSet<Account> Account { get; set; }

        DbSet<AccountAuth> AccountAuth { get; set; }

        DbSet<AccountSession> AccountSession { get; set; }

        DbSet<Permission> Permission { get; set; }

        DbSet<PermissionGroup> PermissionGroup { get; set; }

        DbSet<PermissionValue> PermissionValue { get; set; }

        DbSet<Setting> Setting { get; set; }
    }
}
