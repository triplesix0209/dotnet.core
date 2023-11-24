using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using TripleSix.Core.Appsettings;
using TripleSix.Core.DataContext;
using TripleSix.Core.Mappers;
using TripleSix.Core.Quartz;

namespace Sample.WebApi
{
    public static class Startup
    {
        private static readonly Assembly DomainAssembly = typeof(Domain.AutofacModule).Assembly;
        private static readonly Assembly WebApiAssembly = typeof(WebApi.AutofacModule).Assembly;

        public static async Task<WebApplication> BuildApp(string[] args)
        {
            var configuration = LoadConfiguration(args);

            // dto validators
            BaseValidator.SetupGlobal();
            BaseValidator.ValidateDtoValidator(DomainAssembly);

            // builder & services
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddConfiguration(configuration);
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.ConfigureContainer(configuration));
            builder.Services.ConfigureMvcService(WebApiAssembly);
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSwagger(configuration);
            builder.Services.AddAuthentication().AddJwtAccessToken(configuration);

            // build app
            var app = builder.Build();
            app.ConfigureApp(configuration);
            await app.OnStartup(configuration);
            return app;
        }

        private static IConfiguration LoadConfiguration(string[] args)
        {
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            return new ConfigurationBuilder()
               .AddJsonFile(Path.Combine("Appsettings", "appsettings.json"), true)
               .AddJsonFile(Path.Combine("Appsettings", $"appsettings.{envName}.json"), true)
               .AddEnvironmentVariables()
               .AddCommandLine(args)
               .Build();
        }

        private static void ConfigureContainer(this ContainerBuilder builder, IConfiguration configuration)
        {
            builder.RegisterModule(new Domain.AutofacModule(configuration));
            builder.RegisterModule(new Application.AutofacModule(configuration));
            builder.RegisterModule(new Infrastructure.AutofacModule(configuration));
            builder.RegisterModule(new WebApi.AutofacModule(configuration));
        }

        private static void ConfigureApp(this WebApplication app, IConfiguration configuration)
        {
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.Use404JsonError();
            app.UseReDocUI(configuration);
            app.UseMiddleware<ExceptionMiddleware>();
            app.Use(next => context =>
            {
                context.Request.EnableBuffering();
                return next(context);
            });
            app.MapControllers();
        }

        private static async Task OnStartup(this WebApplication app, IConfiguration configuration)
        {
            using var scope = app.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            // validate mapper profile
            var mapperConfiguration = serviceProvider.GetService<MapperConfiguration>();
            mapperConfiguration?.ValidateConfiguration();

            // apply migrations
            var migrationAppsetting = new MigrationAppsetting(configuration);
            if (migrationAppsetting.ApplyOnStartup)
            {
                var db = serviceProvider.GetRequiredService<IDbMigrationContext>();
                await db.MigrateAsync();
            }

            // start quartz jobs
            serviceProvider.GetRequiredService<JobScheduler>().Start();
        }
    }
}
