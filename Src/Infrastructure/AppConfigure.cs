using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Sample.Infrastructure.Appsettings;
using TripleSix.Core.OpenTelemetry;

namespace Sample.Infrastructure
{
    public static class AppConfigure
    {
        public static void AddInfrastructure(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            AddOpenTelemetry(builder.Services, configuration);
        }

        private static void AddOpenTelemetry(IServiceCollection services, IConfiguration configuration)
        {
            var appsetting = new OpenTelemetryAppsetting(configuration);

            services.AddOpenTelemetryTracing(builder =>
            {
                builder.AddSourceTripleSixCore()
                    .AddSource(appsetting.ServiceName)
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(
                        serviceName: appsetting.ServiceName,
                        serviceVersion: appsetting.ServiceVersion))
                    .AddAspNetCoreInstrumentationEx()
                    .AddEntityFrameworkInstrumentationEx()
                    .AddHttpClientInstrumentationEx();

                if (appsetting.EnableConsoleExporter)
                    builder.AddConsoleExporter();

                if (appsetting.EnableJaegerExporter)
                {
                    builder.AddJaegerExporter(options =>
                    {
                        options.AgentHost = appsetting.JaegerHost;
                        options.AgentPort = appsetting.JaegerPort;
                    });
                }
            });
        }
    }
}
