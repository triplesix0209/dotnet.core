﻿using System.IdentityModel.Tokens.Jwt;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Hangfire;
using Hangfire.Dashboard.Management.v2;
using Microsoft.Data.SqlClient;
using Sample.Domain.DataContext;
using TripleSix.Core.Appsettings;
using TripleSix.Core.Mappers;

namespace Sample.WebApi
{
    public static class Startup
    {
        private static readonly Assembly DomainAssembly = typeof(Domain.AutofacModule).Assembly;
        private static readonly Assembly WebApiAssembly = typeof(WebApi.AutofacModule).Assembly;

        public static async Task<WebApplication> BuildApp(string[] args)
        {
            var configuration = LoadConfiguration(args);

            // builder & services
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddConfiguration(configuration);
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.ConfigureContainer(configuration));
            builder.Services.ConfigureServices(configuration);
            builder.SetupOpenTelemetry(configuration);

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

        private static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            BaseValidator.SetupGlobal();
            BaseValidator.ValidateDtoValidator(DomainAssembly);

            services.AddHttpContextAccessor();
            services.AddSwagger(configuration);
            services.AddMvcServices(WebApiAssembly);
            services.AddAuthentication().AddJwtAccessToken(configuration, GetSigningKey);
            services.AddHangfireWorker(configuration, (options, setting) =>
            {
                options.UseSqlServerStorage(setting.ConnectionString);
                options.UseManagementPages(WebApiAssembly);
            });
        }

        private static void ConfigureApp(this WebApplication app, IConfiguration configuration)
        {
            app.UseMvcService(configuration);
            app.UseReDocUI(configuration);
            app.UseHangfireDashboard(configuration);

            app.UseAuthentication();
            app.UseAuthorization();
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
                var db1 = serviceProvider.GetRequiredService<IApplicationDbContext>();
                await db1.MigrateAsync();

                var db2 = serviceProvider.GetRequiredService<IDataDbContext>();
                await db2.MigrateAsync();
            }

            // setup hangfire
            await serviceProvider.StartHangfire();
        }

        private static string? GetSigningKey(IdentityAppsetting setting, JwtSecurityToken token)
        {
            using var connection = new SqlConnection(setting.ConnectionString);
            connection.Open();

            using var command = new SqlCommand("Select SigningKey From App Where Code = @AppCode", connection);
            command.Parameters.AddWithValue("@AppCode", token.Issuer);

            using var reader = command.ExecuteReader();
            if (reader.Read() == false) return null;

            return reader.GetString(0);
        }
    }
}
