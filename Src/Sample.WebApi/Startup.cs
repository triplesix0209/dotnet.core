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
using TripleSix.Core.Helpers;
using TripleSix.Core.Quartz;
using TripleSix.Core.WebApi;
using TripleSix.Core.WebApi.Swagger;

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
            builder.RegisterModule(new TripleSix.Core.AutoAdmin.AutofacModule(Configuration));
            builder.RegisterModule(new Common.AutofacModule(Configuration));
            builder.RegisterModule(new Data.AutofacModule(Configuration));
            builder.RegisterModule(new Middle.AutofacModule(Configuration));
            builder.RegisterModule(new Quartz.AutofacModule(Configuration));
            builder.RegisterModule(new Controllers.Admins.Methods.AutofacModule(Configuration));
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
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataContext dataContext)
        {
            BaseConfigure(app, env);
            dataContext.Database.Migrate();
            AutofacContainer.Resolve<JobScheduler>().Start();

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
