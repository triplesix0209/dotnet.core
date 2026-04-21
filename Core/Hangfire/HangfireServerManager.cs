#pragma warning disable SA1649 // File name should match first type name

using Hangfire;
using TripleSix.Core.Appsettings;

namespace TripleSix.Core.Hangfire
{
    /// <summary>
    /// Hangfire Worker Manager.
    /// </summary>
    public interface IHangfireServerManager
    {
        /// <summary>
        /// Kiểm tra xem có Hangfire Server nào đang chạy hay không.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Khởi động các server đăng ký trong Appsetting.
        /// </summary>
        void Start();

        /// <summary>
        /// Tắt các server đăng ký trong Appsetting.
        /// </summary>
        void Stop();
    }

    /// <summary>
    /// Hangfire Manager.
    /// </summary>
    public class HangfireServerManager : IHangfireServerManager, IDisposable
    {
        private readonly object _lock = new();
        private readonly List<BackgroundJobServer> _servers = [];

        /// <summary>
        /// Initializes a new instance of the <see cref="HangfireServerManager"/> class.
        /// </summary>
        public HangfireServerManager()
        {
        }

        /// <summary>
        /// <see cref="HangfireAppsetting"/>.
        /// </summary>
        public HangfireAppsetting Setting { get; set; }

        /// <inheritdoc/>
        public bool IsRunning => _servers.Count > 0;

        /// <inheritdoc/>
        public void Dispose() => Stop();

        /// <inheritdoc/>
        public void Start()
        {
            lock (_lock)
            {
                if (IsRunning) return;

                foreach (var queueSetting in Setting.Queues)
                {
                    var options = new BackgroundJobServerOptions
                    {
                        Queues = [queueSetting.Name],
                        WorkerCount = queueSetting.WorkerCount ?? 10,
                    };

                    _servers.Add(new BackgroundJobServer(options));
                }
            }
        }

        /// <inheritdoc/>
        public void Stop()
        {
            lock (_lock)
            {
                foreach (var server in _servers)
                    server.Dispose();
                _servers.Clear();
            }
        }
    }
}
