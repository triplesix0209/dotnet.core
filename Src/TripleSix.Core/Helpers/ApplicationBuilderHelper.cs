using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.ReDoc;

namespace TripleSix.Core.Helpers
{
    public static class ApplicationBuilderHelper
    {
        public static void UseSwagger(this IApplicationBuilder app, IConfiguration configuration, Action<ReDocOptions> setupAction)
        {
            if (configuration.GetValue("Swagger:Enable", false) == false) return;

            app.UseSwagger();
            app.UseReDoc(setupAction);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", context =>
                {
                    context.Response.Redirect("/swagger");
                    return Task.CompletedTask;
                });
            });
        }
    }
}
