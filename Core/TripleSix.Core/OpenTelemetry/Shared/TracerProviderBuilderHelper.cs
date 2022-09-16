using System.Diagnostics;
using System.Reflection;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Trace;

namespace TripleSix.Core.OpenTelemetry.Shared
{
    public class TracerProviderBuilderHelper
    {
        internal static readonly Func<HttpClient> DefaultHttpClientFactory = () => new HttpClient();

        internal static TracerProviderBuilder AddJaegerExporter(TracerProviderBuilder builder, JaegerExporterOptions options, Action<JaegerExporterOptions> configure, IServiceProvider? serviceProvider, Func<Activity, bool>? filter)
        {
            configure.Invoke(options);
            if (serviceProvider != null && options.Protocol == JaegerExportProtocol.HttpBinaryThrift && options.HttpClientFactory == DefaultHttpClientFactory)
            {
                options.HttpClientFactory = () =>
                {
                    var type = Type.GetType("System.Net.Http.IHttpClientFactory, Microsoft.Extensions.Http", throwOnError: false);
                    if (type != null)
                    {
                        var service = serviceProvider.GetService(type);
                        if (service != null)
                        {
                            var method = type.GetMethod("CreateClient", BindingFlags.Instance | BindingFlags.Public, null, new Type[1] { typeof(string) }, null);
                            if (method != null)
                                return (HttpClient?)method.Invoke(service, new object[1] { "JaegerExporter" });
                        }
                    }

                    return new HttpClient();
                };
            }

            var exporter = new JaegerExporter(options);
            BaseProcessor<Activity> exportProcessor = options.ExportProcessorType == ExportProcessorType.Simple
                ? new FilteringSimpleActivityExportProcessor(exporter, filter)
                : new FilteringBatchActivityExportProcessor(exporter, options.BatchExportProcessorOptions.MaxQueueSize, options.BatchExportProcessorOptions.ScheduledDelayMilliseconds, options.BatchExportProcessorOptions.ExporterTimeoutMilliseconds, options.BatchExportProcessorOptions.MaxExportBatchSize, filter);
            return builder.AddProcessor(exportProcessor);
        }
    }
}
