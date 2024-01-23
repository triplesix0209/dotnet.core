using Microsoft.Extensions.Configuration;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Appsettings
{
    /// <summary>
    /// Cấu hình hangfire.
    /// </summary>
    public class HangfireAppsetting : BaseAppsetting
    {
        /// <summary>
        /// Cấu hình hangfire.
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        public HangfireAppsetting(IConfiguration configuration)
            : base(configuration, "Hangfire")
        {
            if (Enable == false) return;

            if (ConnectionString.IsNullOrEmpty()) throw new ArgumentNullException(nameof(ConnectionString));
            if (Queues.IsNullOrEmpty()) throw new ArgumentNullException(nameof(Queues));
            if (WorkerCount <= 0) throw new ArgumentOutOfRangeException(nameof(WorkerCount));

            var config = configuration.GetSection("Hangfire:Dashboard");
            if (config.GetChildren().IsNullOrEmpty()) return;
            DashboardEnable = config.GetValue("Enable", false);
            if (DashboardEnable == false) return;
            DashboardPath = config.GetValue("Path", "/hangfire") ?? "/hangfire";
            DashboardUsername = config.GetValue<string?>("Username", null);
            DashboardPassword = config.GetValue<string?>("Password", null);
            if ((DashboardUsername.IsNullOrEmpty() && DashboardPassword.IsNotNullOrEmpty())
                || (DashboardUsername.IsNotNullOrEmpty() && DashboardPassword.IsNullOrEmpty()))
                throw new ArgumentNullException("Username & Password of Hangfire Dashboard is invalid");
        }

        /// <summary>
        /// Bật/tắt hangfire (mặc định là false).
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

        /// <summary>
        /// Số lượng worker.
        /// </summary>
        public int? WorkerCount { get; set; }

        /// <summary>
        /// Bật/tắt hangfire dashboard (mặc định là false).
        /// </summary>
        public bool DashboardEnable { get; set; } = false;

        /// <summary>
        /// Đường dẫn hangfire dashboard (mặc định là /hangfire).
        /// </summary>
        public string DashboardPath { get; set; } = "/hangfire";

        /// <summary>
        /// Username truy cập vào dashboard.
        /// </summary>
        public string? DashboardUsername { get; set; }

        /// <summary>
        /// Password truy cập vào dashboard.
        /// </summary>
        public string? DashboardPassword { get; set; }
    }
}
