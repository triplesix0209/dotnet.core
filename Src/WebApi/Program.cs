using Autofac.Extensions.DependencyInjection;
using Sample.Infrastructure;

var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var configuration = new ConfigurationBuilder()
   .AddJsonFile(Path.Combine("Config", "appsettings.json"), true)
   .AddJsonFile(Path.Combine("Config", $"appsettings.{envName}.json"), true)
   .AddEnvironmentVariables()
   .AddCommandLine(args)
   .Build();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureAppConfiguration((host, builder) => builder.AddConfiguration(configuration));

var services = builder.Services;
services.AddInfrastructure(configuration);
services.AddControllers();
services.AddEndpointsApiExplorer();

var app = builder.Build();
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();