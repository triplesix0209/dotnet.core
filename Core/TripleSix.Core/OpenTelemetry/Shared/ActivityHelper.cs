using System.Diagnostics;

namespace TripleSix.Core.OpenTelemetry.Shared
{
    internal class ActivityHelper
    {
        public static readonly string SourceName = typeof(EntityFrameworkCoreDiagnosticListener).Assembly.GetName().Name!;

        public static readonly Version Version = typeof(EntityFrameworkCoreDiagnosticListener).Assembly.GetName().Version!;

        public static readonly ActivitySource ActivitySource = new ActivitySource(SourceName, Version.ToString());
    }
}
