using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Sample.Infrastructure.Persistences
{
    public class MigrationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var assembly = typeof(IApplicationDbContext).Assembly;
            var configuration = new ConfigurationBuilder()
               .AddJsonFile(Path.Combine("Config", "appsettings.json"), true)
               .Build();

            return new ApplicationDbContext(assembly, configuration);
        }
    }
}
