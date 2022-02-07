using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TripleSix.Core.Helpers;
using TripleSix.Core.WebApi;

namespace Sample
{
    public class Startup : BaseStartup
    {
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
            : base(Assembly.GetExecutingAssembly(), env, configuration)
        {
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

            options.SwaggerDoc("api", new OpenApiInfo { Title = "API Document", Version = "v1", });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            BaseConfigure(app, env);

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
