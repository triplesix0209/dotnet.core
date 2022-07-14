using System.Diagnostics.Tracing;

namespace TripleSix.Core.OpenTelemetry.Shared
{
    [EventSource(Name = "OpenTelemetry-Instrumentation")]
    internal class InstrumentationEventSource : EventSource
    {
        private static readonly InstrumentationEventSource _log = new ();

        public static InstrumentationEventSource Log => _log;

        [Event(1, Message = "Current Activity is NULL in the '{0}' callback. Activity will not be recorded.", Level = EventLevel.Warning)]
        public void NullActivity(string eventName)
        {
            WriteEvent(1, eventName);
        }

        [NonEvent]
        public void UnknownErrorProcessingEvent(string handlerName, string eventName, Exception ex)
        {
            if (IsEnabled(EventLevel.Error, (EventKeywords)(-1)))
                UnknownErrorProcessingEvent(handlerName, eventName, ex.ToInvariantString());
        }

        [Event(2, Message = "Unknown error processing event '{1}' from handler '{0}', Exception: {2}", Level = EventLevel.Error)]
        public void UnknownErrorProcessingEvent(string handlerName, string eventName, string ex)
        {
            WriteEvent(2, handlerName, eventName, ex);
        }
    }
}
