using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace Sample.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // load configuration
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
               .AddJsonFile(Path.Combine("Appsettings", "appsettings.json"), true)
               .AddJsonFile(Path.Combine("Appsettings", $"appsettings.{envName}.json"), true)
               .AddEnvironmentVariables()
               .AddCommandLine(args)
               .Build();

            // setup builder
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureAppConfiguration((host, builder) => builder.AddConfiguration(configuration));
            builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.ConfigureContainer(configuration));

            // build app & run
            var app = await builder.BuildApp(configuration);
            app.Run();
        }
    }
}