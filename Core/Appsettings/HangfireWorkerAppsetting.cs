using Microsoft.Extensions.Configuration;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Appsettings
{
    /// <summary>
    /// Cấu hình hangfire worker.
    /// </summary>
    public class HangfireWorkerAppsetting : BaseAppsetting
    {
        /// <summary>
        /// Cấu hình elasticsearch.
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        public HangfireWorkerAppsetting(IConfiguration configuration)
            : base(configuration, "HangfireWorker")
        {
            if (Enable == false) return;

            if (ConnectionString.IsNullOrEmpty()) throw new ArgumentNullException(nameof(ConnectionString));
            if (Queues.IsNullOrEmpty()) throw new ArgumentNullException(nameof(Queues));
        }

        /// <summary>
        /// Bật/tắt hangfire worker (mặc định là false).
        /// </summary>
        public bool Enable { get; set; } = false;

        /// <summary>
        /// Database connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Danh sách queue listen.
        /// </summary>
        public string[]? Queues { get; set; }
    }
}
