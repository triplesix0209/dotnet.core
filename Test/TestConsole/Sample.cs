#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

using System.Diagnostics;

namespace TestConsole
{
    internal class SampleClient
    {
        private static readonly DiagnosticSource _logger = new DiagnosticListener("Sample.Client");

        public byte[] SendWebRequest(string url)
        {
            if (_logger.IsEnabled("RequestStart"))
                _logger.Write("RequestStart", new { Url = url });

            var reply = Array.Empty<byte>();
            return reply;
        }
    }

    internal class Observer<T> : IObserver<T>
    {
        private readonly Action _onCompleted;
        private readonly Action<Exception> _onError;
        private readonly Action<T> _onNext;

        public Observer(Action? onCompleted = default, Action<Exception>? onError = default, Action<T>? onNext = default)
        {
            _onCompleted = onCompleted ?? new Action(() => { });
            _onError = onError ?? new Action<Exception>(_ => { });
            _onNext = onNext ?? new Action<T>(_ => { });
        }

        public void OnCompleted()
        {
            _onCompleted();
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(T value)
        {
            _onNext(value);
        }
    }

    internal class SampleListener
    {
        private readonly object _allListeners = new();
        private IDisposable? _networkSubscription;
        private IDisposable? _listenerSubscription;

        public void Listening()
        {
            Action<KeyValuePair<string, object?>> whenHeard = data =>
            {
                Console.WriteLine($"Data received: {data.Key}: {data.Value}");
            };

            Action<DiagnosticListener> onNewListener = listener =>
            {
                Console.WriteLine($"New Listener discovered: {listener.Name}");

                if (listener.Name == "Sample.Client")
                {
                    lock (_allListeners)
                    {
                        if (_networkSubscription != null)
                            _networkSubscription.Dispose();

                        var iobserver = new Observer<KeyValuePair<string, object?>>(onNext: whenHeard);
                        _networkSubscription = listener.Subscribe(iobserver);
                    }
                }
            };

            var observer = new Observer<DiagnosticListener>(onNext: onNewListener);
            _listenerSubscription = DiagnosticListener.AllListeners.Subscribe(observer);
        }
    }
}
