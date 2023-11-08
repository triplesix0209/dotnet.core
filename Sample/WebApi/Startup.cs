using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using TripleSix.Core.Appsettings;
using TripleSix.Core.DataContext;
using TripleSix.Core.Quartz;

namespace Sample.WebApi
{
    public static class Startup
    {
        public static void ConfigureContainer(this ContainerBuilder builder, IConfiguration configuration)
        {
            builder.RegisterModule(new Domain.AutofacModule(configuration));
            builder.RegisterModule(new Application.AutofacModule(configuration));
            builder.RegisterModule(new Infrastructure.AutofacModule(configuration));
            builder.RegisterModule(new WebApi.AutofacModule(configuration));
        }

        public static async Task<WebApplication> BuildApp(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            var assembly = Assembly.GetExecutingAssembly();
            builder.Services.ConfigureMvcService(assembly);
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAuthentication().AddJwtAccessToken(configuration);
            builder.Services.AddSwagger(configuration);

            #region [build app]

            var app = builder.Build();

            var autofacContainer = app.Services.GetAutofacRoot();
            if (autofacContainer.IsRegistered<MapperConfiguration>())
                autofacContainer.Resolve<MapperConfiguration>().AssertConfigurationIsValid();

            BaseValidator.SetupGlobal();
            BaseValidator.ValidateDtoValidator(typeof(Domain.AutofacModule).Assembly);

            app.UseRouting();
            app.UseHttpsRedirection();
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

            #endregion

            #region [startup action]

            using (var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

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

            #endregion

            return app;
        }
    }
}
