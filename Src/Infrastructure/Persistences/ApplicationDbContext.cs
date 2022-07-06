using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TripleSix.Core.Persistences;

namespace Sample.Infrastructure.Persistences
{
    public class ApplicationDbContext : BaseDbContext, IApplicationDbContext
    {
        private IConfiguration _configuration;

        public ApplicationDbContext(Assembly assembly, IConfiguration configuration)
            : base(assembly)
        {
            _configuration = configuration;
        }

        public DbSet<Account>? Account { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);

            builder.UseNpgsql(_configuration.GetConnectionString("Default"));
        }
    }
}
