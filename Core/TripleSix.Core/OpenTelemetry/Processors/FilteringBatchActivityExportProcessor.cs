using System.Diagnostics;
using OpenTelemetry;

namespace TripleSix.Core.OpenTelemetry
{
    public class FilteringBatchActivityExportProcessor : BatchActivityExportProcessor
    {
        private readonly Func<Activity, bool>? filter;

        public FilteringBatchActivityExportProcessor(BaseExporter<Activity> exporter, int maxQueueSize = 2048, int scheduledDelayMilliseconds = 5000, int exporterTimeoutMilliseconds = 30000, int maxExportBatchSize = 512, Func<Activity, bool>? filter = null)
            : base(exporter, maxQueueSize, scheduledDelayMilliseconds, exporterTimeoutMilliseconds, maxExportBatchSize)
        {
            this.filter = filter;
        }

        public override void OnEnd(Activity data)
        {
            if (filter != null && !filter(data)) return;
            base.OnEnd(data);
        }
    }
}
