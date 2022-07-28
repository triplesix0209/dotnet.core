﻿using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.OpenApi.Models;
using Sample.Infrastructure.Startup;
using TripleSix.Core.Appsettings;
using TripleSix.Core.AutofacModules;
using TripleSix.Core.Persistences;
using TripleSix.Core.Validation;

namespace Sample.WebApi
{
    public static class Startup
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
            builder.AddInfrastructure(configuration);
            builder.Services.AddHttpContextAccessor();
            builder.Services.ConfigureMvcService();

            #region [authentication]

            builder.Services
                .AddAuthentication(option =>
                {
                    option.DefaultAuthenticateScheme = "access-token";
                    option.DefaultChallengeScheme = "access-token";
                })
                .AddScheme<AccessTokenSchemeOption, AccessTokenSchemeHandler>("access-token", option => { });

            #endregion

            #region [swagger]

            builder.Services.AddSwagger(configuration, (options, appsetting) =>
            {
                options.SwaggerDoc("common", new OpenApiInfo { Title = "Common API Document", Version = "1.0" });
                options.SwaggerDoc("admin", new OpenApiInfo { Title = "Admin API Document", Version = "1.0" });

                options.AddSecurityDefinition("access-token", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Description = "Nhập access token vào field của header để tiến hành xác thực",
                });

                options.OperationFilter<DescribeOperationFilter>("access-token");
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