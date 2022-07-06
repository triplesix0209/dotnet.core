using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Sample.Infrastructure.Persistences;
using TripleSix.Core.Interfaces.DbContext;

var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var configuration = new ConfigurationBuilder()
   .AddJsonFile(Path.Combine("Config", "appsettings.json"), true)
   .AddJsonFile(Path.Combine("Config", $"appsettings.{envName}.json"), true)
   .AddEnvironmentVariables()
   .AddCommandLine(args)
   .Build();

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureAppConfiguration((host, builder) => builder.AddConfiguration(configuration));

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.Register(c => new ApplicationDbContext(Assembly.GetExecutingAssembly(), configuration))
        .AsImplementedInterfaces()
        .InstancePerLifetimeScope();
});

var services = builder.Services;
services.AddMvc().AddControllersAsServices();

var app = builder.Build();
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<IDbMigrationContext>();
    await db.MigrateAsync();
}

app.Run();
