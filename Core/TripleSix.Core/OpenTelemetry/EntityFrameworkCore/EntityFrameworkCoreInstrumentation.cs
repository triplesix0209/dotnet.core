using TripleSix.Core.OpenTelemetry.Shared;

namespace TripleSix.Core.OpenTelemetry
{
    internal class EntityFrameworkCoreInstrumentation : IDisposable
    {
        private readonly DiagnosticSourceSubscriber _diagnosticSourceSubscriber;

        public EntityFrameworkCoreInstrumentation(EntityFrameworkCoreInstrumentationOptions options)
        {
            _diagnosticSourceSubscriber = new DiagnosticSourceSubscriber(
               name => new EntityFrameworkCoreDiagnosticListener(name, options),
               listener => listener.Name == EntityFrameworkCoreDiagnosticListener.DiagnosticSourceName,
               null);
            _diagnosticSourceSubscriber.Subscribe();
        }

        public void Dispose()
        {
            _diagnosticSourceSubscriber.Dispose();
        }
    }
}
