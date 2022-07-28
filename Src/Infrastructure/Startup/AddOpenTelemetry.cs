using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using TripleSix.Core.Appsettings;
using TripleSix.Core.OpenTelemetry;

namespace Sample.Infrastructure.Startup
{
    public static partial class Extension
    {
        private static void AddOpenTelemetry(IServiceCollection services, IConfiguration configuration)
        {
            var appsetting = new OpenTelemetryAppsetting(configuration);

            services.AddOpenTelemetryTracing(builder =>
            {
                builder
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(
                        serviceName: appsetting.ServiceName,
                        serviceVersion: appsetting.ServiceVersion))
                    .AddSource(appsetting.ServiceName)
                    .AddSourceTripleSixCore()
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
