using System.Diagnostics;
using OpenTelemetry;

namespace TripleSix.Core.OpenTelemetry
{
    public class FilteringSimpleActivityExportProcessor : SimpleActivityExportProcessor
    {
        private readonly Func<Activity, bool>? filter;

        public FilteringSimpleActivityExportProcessor(BaseExporter<Activity> exporter, Func<Activity, bool>? filter = null)
            : base(exporter)
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
