using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TripleSix.Core.Persistences;

namespace Sample.Infrastructure.Persistences
{
    public partial class ApplicationDbContext : BaseDbContext
    {
        private IConfiguration _configuration;

        public ApplicationDbContext(Assembly assembly, IConfiguration configuration)
            : base(assembly)
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
