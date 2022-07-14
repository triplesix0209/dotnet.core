using System.Diagnostics;

namespace TripleSix.Core.OpenTelemetry.Shared
{
    internal abstract class ListenerHandler
    {
        public ListenerHandler(string sourceName)
        {
            SourceName = sourceName;
        }

        public string SourceName { get; }

        public virtual bool SupportsNullActivity => false;

        public virtual void OnStartActivity(Activity? activity, object? payload)
        {
        }

        public virtual void OnStopActivity(Activity? activity, object? payload)
        {
        }

        public virtual void OnException(Activity? activity, object? payload)
        {
        }

        public virtual void OnCustom(string eventName, Activity? activity, object? payload)
        {
        }
    }
}
