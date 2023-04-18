using Microsoft.Extensions.Configuration;

namespace TripleSix.Core.Appsettings
{
    public abstract class BaseAppsetting
    {
        protected BaseAppsetting(IConfiguration configuration, string key)
        {
            var data = configuration.GetSection(key).GetChildren();
            foreach (var prop in GetType().GetProperties())
            {
                if (!data.Any(x => x.Key == prop.Name)) continue;
                var value = data.First(x => x.Key == prop.Name).Get(prop.PropertyType);
                prop.SetValue(this, value);
            }
        }
    }
}
