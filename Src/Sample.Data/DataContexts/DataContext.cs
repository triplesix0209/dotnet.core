using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sample.Data.Entities;
using TripleSix.Core.DataContexts;

namespace Sample.Data.DataContexts
{
    public partial class DataContext : PostgresqlContext
    {
        public DataContext(IConfiguration configuration)
            : base(Assembly.GetExecutingAssembly(), configuration)
        {
        }

        public DbSet<SettingEntity> Setting { get; set; }
    }
}
