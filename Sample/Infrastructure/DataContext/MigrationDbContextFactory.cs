using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Sample.Infrastructure.DataContext
{
    public class MigrationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile(Path.Combine("Config", "appsettings.json"), true)
               .Build();

            return new ApplicationDbContext(configuration);
        }
    }
}
