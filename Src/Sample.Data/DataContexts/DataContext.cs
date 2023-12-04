using System.Reflection;
using Sample.Data.Entities;

namespace Sample.Data.DataContexts
{
    public partial class DataContext : PostgresqlContext
    {
        public DataContext(IConfiguration configuration)
            : base(Assembly.GetExecutingAssembly(), configuration)
        {
        }

        public DbSet<AccountAuthEntity> AccountAuth { get; set; }

        public DbSet<AccountEntity> Account { get; set; }

        public DbSet<AccountSessionEntity> AccountSession { get; set; }

        public DbSet<AccountVerifyEntity> AccountVerify { get; set; }

        public DbSet<PermissionGroupEntity> PermissionGroup { get; set; }

        public DbSet<PermissionValueEntity> PermissionValue { get; set; }

        public DbSet<PermissionEntity> Permission { get; set; }

        public DbSet<SettingEntity> Setting { get; set; }

        public DbSet<ObjectLogEntity> ObjectLog { get; set; }

        public DbSet<TestEntity> Test { get; set; }
    }
}
