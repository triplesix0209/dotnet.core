using Microsoft.Extensions.Configuration;

namespace CpTech.Core.Extensions
{
    public static class ConfigurationExtension
    {
        public static bool IsProductionEnv(this IConfiguration configuration)
        {
            return configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT") == "Production";
        }
    }
}