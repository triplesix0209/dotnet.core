using Autofac;
using Autofac.Extensions.DependencyInjection;
using TripleSix.Core.Appsettings;
using TripleSix.Core.Persistences;

namespace Sample.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
               .AddJsonFile(Path.Combine("Config", "appsettings.json"), true)
               .AddJsonFile(Path.Combine("Config", $"appsettings.{envName}.json"), true)
               .AddEnvironmentVariables()
               .AddCommandLine(args)
               .Build();

            // build host
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.ConfigureAppConfiguration((host, builder) => builder.AddConfiguration(configuration));
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.ConfigureContainer(configuration));
            builder.Services.ConfigureService(configuration);

            // build app
            var app = builder.Build();
            app.ConfigureApp(configuration);

            // startup action
            using (var scope = app.Services.CreateScope())
            {
                var migrationAppsetting = new MigrationAppsetting(configuration);
                if (migrationAppsetting.ApplyOnStartup)
                {
                    var db = scope.ServiceProvider.GetRequiredService<IDbMigrationContext>();
                    await db.MigrateAsync();
                }
            }

            app.Run();
        }
    }
}
