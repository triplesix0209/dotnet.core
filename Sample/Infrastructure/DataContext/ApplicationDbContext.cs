using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sample.Domain.DataContext;

namespace Sample.Infrastructure.DataContext
{
    public partial class ApplicationDbContext : BaseDbContext
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(IConfiguration configuration)
            : base(typeof(IApplicationDbContext).Assembly, Assembly.GetExecutingAssembly())
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);

            //var serverVersion = new MySqlServerVersion(new Version(8, 0));
            //builder.UseMySql(_configuration.GetConnectionString("Default"), serverVersion);

            builder.UseSqlServer(_configuration.GetConnectionString("Default"));

#if DEBUG
            builder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
#endif
        }
    }
}
