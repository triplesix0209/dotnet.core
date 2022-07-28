using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Sample.Infrastructure.Startup
{
    public static partial class Extension
    {
        public static void AddInfrastructure(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            AddOpenTelemetry(builder.Services, configuration);
        }
    }
}
