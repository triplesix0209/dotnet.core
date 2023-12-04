using System;
using System.IO;
using Sample.Data.DataContexts;

namespace Sample.Data
{
    public class MigrationDataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var config = new ConfigurationBuilder()
               .AddJsonFile(Path.Combine("Config", "appsettings.json"), true)
               .AddJsonFile(Path.Combine("Config", $"appsettings.{envName}.json"), true)
               .AddEnvironmentVariables()
               .AddCommandLine(args)
               .Build();

            return new DataContext(config);
        }
    }
}
