using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.OpenApi.Models;
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

        public static void ConfigureMvc(MvcOptions options)
        {
        }

        public static void ConfigureApplicationPartManager(ApplicationPartManager options)
        {
        }

        public static async Task<WebApplication> BuildApp(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            var assembly = Assembly.GetExecutingAssembly();
            builder.Services.AddHttpContextAccessor();
            builder.Services.ConfigureMvcService(ConfigureMvc, ConfigureApplicationPartManager);

            #region [swagger]

            builder.Services.AddSwagger(configuration, (options, appsetting) =>
            {
                options.SwaggerDoc("app", new OpenApiInfo { Title = "App API Document", Version = "1.0" });
                options.SwaggerDoc("admin", new OpenApiInfo { Title = "Admin API Document", Version = "1.0" });

                options.AddSecurityDefinition("access-token", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Description = "Nhập access token vào field của header để tiến hành xác thực",
                });
            });

            #endregion

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
            app.UseMiddleware<ExceptionMiddleware>();

            app.Use(next => context =>
            {
                context.Request.EnableBuffering();
                return next(context);
            });
            app.MapControllers();

            #endregion

            #region [redoc]

            var swaggerAppsetting = new SwaggerAppsetting(configuration);
            if (swaggerAppsetting.Enable)
            {
                app.UseSwagger();
                app.UseReDoc(options =>
                {
                    options.RoutePrefix = swaggerAppsetting.Route;
                    options.IndexStream = () =>
                    {
                        var assembly = AppDomain.CurrentDomain.GetAssemblies()
                            .First(x => x.GetName().Name == AppDomain.CurrentDomain.FriendlyName);

                        var redocStream = assembly.GetManifestResourceNames()
                            .First(x => x.EndsWith("ReDoc.html"));

                        return assembly.GetManifestResourceStream(redocStream);
                    };
                });

                app.MapGet("/", context =>
                {
                    context.Response.Redirect("/" + swaggerAppsetting.Route);
                    return Task.CompletedTask;
                });
            }

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
