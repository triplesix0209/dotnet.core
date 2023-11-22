using Microsoft.Extensions.Configuration;

namespace TripleSix.Core.Appsettings
{
    /// <summary>
    /// Cấu hình database migration.
    /// </summary>
    public class MigrationAppsetting : BaseAppsetting
    {
        /// <summary>
        /// Cấu hình database migration.
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        public MigrationAppsetting(IConfiguration configuration)
            : base(configuration, "Migration")
        {
        }

        /// <summary>
        /// Chạy migrate khi khởi động.
        /// </summary>
        public bool ApplyOnStartup { get; set; } = false;
    }
}
