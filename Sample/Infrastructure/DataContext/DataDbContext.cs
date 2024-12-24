using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Sample.Infrastructure.DataContext
{
    public partial class DataDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DataDbContext(IConfiguration configuration)
        //: base(typeof(IApplicationDbContext).Assembly, Assembly.GetExecutingAssembly())
        {
            _configuration = configuration;
        }

        /// <inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);

            builder.UseSqlServer(_configuration.GetConnectionString("Default"));

#if DEBUG
            builder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
#endif
        }
    }
}
