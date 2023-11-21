using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Sample.Infrastructure.DataContext
{
    public class MigrationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        /// <inheritdoc/>
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile(Path.Combine("Appsettings", "appsettings.json"), true)
               .Build();

            return new ApplicationDbContext(configuration);
        }
    }
}
