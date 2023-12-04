using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Sample.Common;
using Sample.Data.DataContexts;
using Swashbuckle.AspNetCore.SwaggerGen;
using TripleSix.CoreOld.Helpers;
using TripleSix.CoreOld.Quartz;
using TripleSix.CoreOld.WebApi;
using TripleSix.CoreOld.WebApi.Authentication;
using TripleSix.CoreOld.WebApi.Swagger;

namespace Sample.WebApi
{
    public class Startup : BaseStartup
    {
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
            : base(Assembly.GetExecutingAssembly(), env, configuration)
        {
        }

        public override void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);
            builder.RegisterModule(new TripleSix.CoreOld.AutoAdmin.AutofacModule(Configuration));
            builder.RegisterModule(new Common.AutofacModule(Configuration));
            builder.RegisterModule(new Data.AutofacModule(Configuration));
            builder.RegisterModule(new Middle.AutofacModule(Configuration));
            builder.RegisterModule(new Quartz.AutofacModule(Configuration));
            builder.RegisterModule(new Controllers.Admins.Auto.AutofacModule(Configuration));
        }

        public override void ConfigureCors(CorsOptions options)
        {
            options.AddDefaultPolicy(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
        }

        public override void ConfigureSwagger(SwaggerGenOptions options)
        {
            base.ConfigureSwagger(options);

            options.SwaggerDoc("common", new OpenApiInfo { Title = "API Document", Version = "1.0" });
            options.SwaggerDoc("admin", new OpenApiInfo { Title = "Admin API Document", Version = "1.0" });

            options.OperationFilter<IdentityOperationFilter<Identity>>();
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddDbContext<DataContext>();

            services.AddAuthentication(option =>
            {
                option.DefaultForbidScheme = "account-token";
            }).AddTokenScheme("account-token");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataContext dataContext)
        {
            BaseConfigure(app, env);
            dataContext.Database.Migrate();
            AutofacContainer.Resolve<JobScheduler>().Start();
            TripleSix.CoreOld.AutoAdmin.BaseAdminMetadataController.Validate();

            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseSwagger(Configuration, ConfigureReDoc);

            if (!env.IsEnvironment("Local"))
            {
                app.UseHttpsRedirection();
                app.UseHsts();
            }
        }
    }
}
