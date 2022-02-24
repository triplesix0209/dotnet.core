using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sample.Data.Entities;
using TripleSix.Core.AutoAdmin;
using TripleSix.Core.DataContexts;

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
    }
}
