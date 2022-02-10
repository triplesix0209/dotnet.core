using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Sample.Common;
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
            builder.RegisterModule(new Common.AutofacModule(Configuration));
            builder.RegisterModule(new Data.AutofacModule(Configuration));
            builder.RegisterModule(new Middle.AutofacModule(Configuration));
            builder.RegisterModule(new Quartz.AutofacModule(Configuration));
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

            options.SwaggerDoc("api", new OpenApiInfo { Title = "API Document", Version = "1.0" });

            options.OperationFilter<IdentityOperationFilter<Identity>>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            BaseConfigure(app, env);
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
