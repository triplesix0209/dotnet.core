using Microsoft.Extensions.Configuration;

namespace TripleSix.Core.Extensions
{
    public static class ConfigurationExtension
    {
        public static bool IsProductionEnv(this IConfiguration configuration)
        {
            return configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT") == "Production";
        }
    }
}