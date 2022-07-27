using Microsoft.EntityFrameworkCore;
using Sample.Domain.Entities;
using Sample.Domain.Persistences;

namespace Sample.Infrastructure.Persistences
{
    public partial class ApplicationDbContext : IApplicationDbContext
    {
        public DbSet<Account> Account { get; set; }

        public DbSet<AccountAuth> AccountAuth { get; set; }

        public DbSet<AccountSession> AccountSession { get; set; }

        public DbSet<Permission> Permission { get; set; }

        public DbSet<PermissionGroup> PermissionGroup { get; set; }

        public DbSet<PermissionValue> PermissionValue { get; set; }

        public DbSet<Setting> Setting { get; set; }
    }
}
