using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Sample.Infrastructure.Appsettings;

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
            var openTelemetryAppsetting = new OpenTelemetryAppsetting(configuration);

            services.AddOpenTelemetryTracing(tracerProviderBuilder =>
            {
                tracerProviderBuilder
                    .AddSource(openTelemetryAppsetting.ServiceName)
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(
                        serviceName: openTelemetryAppsetting.ServiceName,
                        serviceVersion: openTelemetryAppsetting.ServiceVersion))
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.Enrich = (activity, eventName, rawObject) =>
                        {
                            if (eventName.Equals("OnStartActivity"))
                            {
                                if (rawObject is HttpRequest httpRequest)
                                    activity.SetTag("requestProtocol", httpRequest.Protocol);
                            }
                            else if (eventName.Equals("OnStopActivity"))
                            {
                                if (rawObject is HttpResponse httpResponse)
                                    activity.SetTag("responseLength", httpResponse.ContentLength);
                            }
                        };
                    })
                    .AddEntityFrameworkCoreInstrumentation(options =>
                    {
                        options.SetDbStatementForText = true;
                        options.SetDbStatementForStoredProcedure = true;
                    })
                    .AddConsoleExporter();
            });
        }
    }
}
