using Microsoft.Extensions.Configuration;

namespace TripleSix.Core.Appsettings
{
    public class MigrationAppsetting : BaseAppsetting
    {
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
