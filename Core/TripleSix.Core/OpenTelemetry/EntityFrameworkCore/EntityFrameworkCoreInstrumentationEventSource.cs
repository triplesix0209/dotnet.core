using System.Diagnostics.Tracing;
using System.Globalization;

namespace TripleSix.Core.OpenTelemetry
{
    [EventSource(Name = "TripleSixCore-Instrumentation-EntityFrameworkCore")]
    internal class EntityFrameworkCoreInstrumentationEventSource : EventSource
    {
        private static readonly EntityFrameworkCoreInstrumentationEventSource _log = new ();

        public static EntityFrameworkCoreInstrumentationEventSource Log => _log;

        [NonEvent]
        public void UnknownErrorProcessingEvent(string handlerName, string eventName, Exception ex)
        {
            if (IsEnabled(EventLevel.Error, (EventKeywords)(-1)))
                UnknownErrorProcessingEvent(handlerName, eventName, ToInvariantString(ex));
        }

        [Event(1, Message = "Unknown error processing event '{1}' from handler '{0}', Exception: {2}", Level = EventLevel.Error)]
        public void UnknownErrorProcessingEvent(string handlerName, string eventName, string ex)
        {
            WriteEvent(1, handlerName, eventName, ex);
        }

        [Event(2, Message = "Current Activity is NULL the '{0}' callback. Span will not be recorded.", Level = EventLevel.Warning)]
        public void NullActivity(string eventName)
        {
            WriteEvent(2, eventName);
        }

        [Event(3, Message = "Payload is NULL in event '{1}' from handler '{0}', span will not be recorded.", Level = EventLevel.Warning)]
        public void NullPayload(string handlerName, string eventName)
        {
            WriteEvent(3, handlerName, eventName);
        }

        [Event(4, Message = "Payload is invalid in event '{1}' from handler '{0}', span will not be recorded.", Level = EventLevel.Warning)]
        public void InvalidPayload(string handlerName, string eventName)
        {
            WriteEvent(4, handlerName, eventName);
        }

        private static string ToInvariantString(Exception exception)
        {
            var originalUICulture = Thread.CurrentThread.CurrentUICulture;

            try
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
                return exception.ToString();
            }
            finally
            {
                Thread.CurrentThread.CurrentUICulture = originalUICulture;
            }
        }
    }
}
