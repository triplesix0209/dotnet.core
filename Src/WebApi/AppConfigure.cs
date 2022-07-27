using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.OpenApi.Models;
using Sample.Infrastructure;
using TripleSix.Core.Appsettings;
using TripleSix.Core.AutofacModules;
using TripleSix.Core.Persistences;
using TripleSix.Core.Validators;

namespace Sample.WebApi
{
    public static class AppConfigure
    {
        public static void ConfigureContainer(this ContainerBuilder builder, IConfiguration configuration)
        {
            var assembly = Assembly.GetExecutingAssembly();

            builder.RegisterModule(new Domain.AutofacModule(configuration));
            builder.RegisterModule(new Application.AutofacModule(configuration));
            builder.RegisterModule(new Infrastructure.AutofacModule(configuration));

            builder.RegisterAllController(assembly);
        }

        public static async Task<WebApplication> BuildApp(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            builder.Services.AddHttpContextAccessor();
            builder.Services.ConfigureMvcService();
            builder.AddInfrastructure(configuration);

            #region [swagger]

            var swaggerOption = new SwaggerAppsetting(configuration);
            if (swaggerOption.Enable)
            {
                builder.Services.AddSwaggerGen(options =>
                {
                    options.SwaggerGeneratorOptions.DescribeAllParametersInCamelCase = true;
                    options.CustomSchemaIds(x => x.FullName);
                    options.EnableAnnotations();

                    options.SwaggerDoc("common", new OpenApiInfo { Title = "Common API Document", Version = "1.0" });
                    options.SwaggerDoc("admin", new OpenApiInfo { Title = "Admin API Document", Version = "1.0" });
                });
            }

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
            app.UseAuthorization();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

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

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapGet("/", context =>
                    {
                        context.Response.Redirect("/" + swaggerAppsetting.Route);
                        return Task.CompletedTask;
                    });
                });
            }

            #endregion

            #region [startup action]

            using (var scope = app.Services.CreateScope())
            {
                var migrationAppsetting = new MigrationAppsetting(configuration);
                if (migrationAppsetting.ApplyOnStartup)
                {
                    var db = scope.ServiceProvider.GetRequiredService<IDbMigrationContext>();
                    await db.MigrateAsync();
                }
            }

            #endregion

            return app;
        }
    }
}
