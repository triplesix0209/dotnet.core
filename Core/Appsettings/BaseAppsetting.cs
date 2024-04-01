using Microsoft.Extensions.Configuration;

namespace TripleSix.Core.Appsettings
{
    /// <summary>
    /// Cấu hình mặc định.
    /// </summary>
    public abstract class BaseAppsetting
    {
        /// <summary>
        /// Cấu hình mặc định.
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        /// <param name="key">Key mục load dữ liệu.</param>
        protected BaseAppsetting(IConfiguration configuration, string key)
        {
            Configuration = configuration;
            var data = configuration.GetSection(key).GetChildren();
            foreach (var prop in GetType().GetProperties())
            {
                if (!data.Any(x => x.Key == prop.Name)) continue;
                var value = data.First(x => x.Key == prop.Name).Get(prop.PropertyType);
                prop.SetValue(this, value);
            }
        }

        /// <summary>
        /// <see cref="IConfiguration"/>.
        /// </summary>
        public IConfiguration Configuration { get; }
    }
}
