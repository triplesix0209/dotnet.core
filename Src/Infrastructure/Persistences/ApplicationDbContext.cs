#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.Reflection;
using Microsoft.Extensions.Configuration;
using Sample.Domain.Persistences;

namespace Sample.Infrastructure.Persistences
{
    public partial class ApplicationDbContext : BaseDbContext
    {
        private IConfiguration _configuration;

        public ApplicationDbContext(IConfiguration configuration)
            : base(typeof(IApplicationDbContext).Assembly, Assembly.GetExecutingAssembly())
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);

            var serverVersion = new MySqlServerVersion(new Version(8, 0));
            builder.UseMySql(_configuration.GetConnectionString("Default"), serverVersion);
        }
    }
}
