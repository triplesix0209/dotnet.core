using System.Diagnostics;

namespace TripleSix.Core.OpenTelemetry.Shared
{
    public static class ActivityHelper
    {
        internal static readonly string SourceName = typeof(EntityFrameworkCoreDiagnosticListener).Assembly.GetName().Name!;

        internal static readonly Version Version = typeof(EntityFrameworkCoreDiagnosticListener).Assembly.GetName().Version!;

        internal static readonly ActivitySource ActivitySource = new ActivitySource(SourceName, Version.ToString());

        public static void SetNoExport(this Activity activity, bool value = true)
        {
            activity.SetTag("span.no_export", value);
        }

        public static bool IsNoExport(this Activity activity)
        {
            return activity.GetTagItem("span.no_export") as bool? == true;
        }
    }
}
