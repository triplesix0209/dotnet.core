using System.Diagnostics;

namespace TripleSix.Core.OpenTelemetry.Shared
{
    internal class DiagnosticSourceListener : IObserver<KeyValuePair<string, object?>>
    {
        private readonly ListenerHandler _handler;

        public DiagnosticSourceListener(ListenerHandler handler)
        {
            _handler = handler;
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(KeyValuePair<string, object?> value)
        {
            if (!_handler.SupportsNullActivity && Activity.Current == null)
                return;

            try
            {
                if (value.Key.EndsWith("Start", StringComparison.Ordinal))
                    _handler.OnStartActivity(Activity.Current, value.Value);
                else if (value.Key.EndsWith("Stop", StringComparison.Ordinal))
                    _handler.OnStopActivity(Activity.Current, value.Value);
                else if (value.Key.EndsWith("Exception", StringComparison.Ordinal))
                    _handler.OnException(Activity.Current, value.Value);
                else
                    _handler.OnCustom(value.Key, Activity.Current, value.Value);
            }
            catch (Exception ex)
            {
                InstrumentationEventSource.Log.UnknownErrorProcessingEvent(_handler.SourceName, value.Key, ex);
            }
        }
    }
}
