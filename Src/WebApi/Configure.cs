using System.Reflection;
using Autofac;
using Microsoft.OpenApi.Models;
using TripleSix.Core.Appsettings;
using TripleSix.Core.AutofacModules;

namespace Sample.WebApi
{
    public static class Configure
    {
        public static void ConfigureContainer(this ContainerBuilder builder, IConfiguration configuration)
        {
            var assembly = Assembly.GetExecutingAssembly();

            builder.RegisterAllController(assembly);
            builder.RegisterModule(new Domain.AutofacModule(configuration));
            builder.RegisterModule(new Application.AutofacModule(configuration));
            builder.RegisterModule(new Infrastructure.AutofacModule(configuration, assembly));
        }

        public static void ConfigureService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMvc().AddControllersAsServices();

            // config swagger
            var swaggerOption = new SwaggerAppsetting(configuration);
            if (swaggerOption.Enable)
            {
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerGeneratorOptions.DescribeAllParametersInCamelCase = true;
                    options.CustomSchemaIds(x => x.FullName);
                    options.EnableAnnotations();

                    options.SwaggerDoc("common", new OpenApiInfo { Title = "Common API Document", Version = "1.0" });
                    options.SwaggerDoc("admin", new OpenApiInfo { Title = "Admin API Document", Version = "1.0" });
                });
            }
        }

        public static void ConfigureApp(this WebApplication app, IConfiguration configuration)
        {
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

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
        }
    }
}
