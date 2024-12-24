using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Sample.Infrastructure.DataContext
{
    public class DataDbContextFactory : IDesignTimeDbContextFactory<DataDbContext>
    {
        /// <inheritdoc/>
        public DataDbContext CreateDbContext(string[] args)
        {
            //var configuration = new ConfigurationBuilder()
            //   .AddJsonFile(Path.Combine("Appsettings", "appsettings.json"), true)
            //   .Build();

            return new DataDbContext();
        }
    }
}
