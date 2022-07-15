#pragma warning disable SA1402 // File may only contain a single type

using System.Diagnostics;

namespace TestConsole
{
    internal class HTTPClient
    {
        private static DiagnosticSource httpLogger = new DiagnosticListener("System.Net.Http");

        public byte[] SendWebRequest(string url)
        {
            if (httpLogger.IsEnabled("RequestStart"))
                httpLogger.Write("RequestStart", new { Url = url });

            byte[] reply = new byte[] { };
            return reply;
        }
    }

    internal class Observer<T> : IObserver<T>
    {
        private Action<T> _onNext;
        private Action _onCompleted;

        public Observer(Action<T> onNext, Action onCompleted)
        {
            _onNext = onNext ?? new Action<T>(_ => { });
            _onCompleted = onCompleted ?? new Action(() => { });
        }

        public void OnCompleted()
        {
            _onCompleted();
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(T value) {
            _onNext(value);
        }
    }

    class MyListener
    {
        IDisposable networkSubscription;
        IDisposable listenerSubscription;
        private readonly object allListeners = new();
        public void Listening()
        {
            Action<KeyValuePair<string, object>> whenHeard = delegate (KeyValuePair<string, object> data)
            {
                Console.WriteLine($"Data received: {data.Key}: {data.Value}");
            };
            Action<DiagnosticListener> onNewListener = delegate (DiagnosticListener listener)
            {
                Console.WriteLine($"New Listener discovered: {listener.Name}");
                //Suscribe to the specific DiagnosticListener of interest.
                if (listener.Name == "System.Net.Http")
                {
                    //Use lock to ensure the callback code is thread safe.
                    lock (allListeners)
                    {
                        if (networkSubscription != null)
                        {
                            networkSubscription.Dispose();
                        }
                        IObserver<KeyValuePair<string, object>> iobserver = new Observer<KeyValuePair<string, object>>(whenHeard, null);
                        networkSubscription = listener.Subscribe(iobserver);
                    }

                }
            };
            //Subscribe to discover all DiagnosticListeners
            IObserver<DiagnosticListener> observer = new Observer<DiagnosticListener>(onNewListener, null);
            //When a listener is created, invoke the onNext function which calls the delegate.
            listenerSubscription = DiagnosticListener.AllListeners.Subscribe(observer);
        }
        // Typically you leave the listenerSubscription subscription active forever.
        // However when you no longer want your callback to be called, you can
        // call listenerSubscription.Dispose() to cancel your subscription to the IObservable.
    }
}
