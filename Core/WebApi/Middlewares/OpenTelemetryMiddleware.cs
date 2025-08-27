using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TripleSix.Core.Appsettings;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Middleware ghi nhận curl.
    /// </summary>
    public class OpenTelemetryMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly OpenTelemetryAppsetting _openTelemetryAppsetting;

        /// <summary>
        /// Exception middleware.
        /// </summary>
        /// <param name="next">Next action.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        public OpenTelemetryMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _openTelemetryAppsetting = new OpenTelemetryAppsetting(configuration);
        }

        /// <summary>
        /// Middleware process.
        /// </summary>
        /// <param name="httpContext"><see cref="HttpContext"/>.</param>
        /// <returns>Task process.</returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                Activity.Current?.SetTag("curl", await httpContext.Request.ToCurl());
            }
            catch
            {
            }

            await _next(httpContext);
        }
    }
}
