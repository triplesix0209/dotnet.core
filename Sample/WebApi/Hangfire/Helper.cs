using Hangfire;
using Hangfire.Common;
using Hangfire.Dashboard;

namespace Sample.WebApi.Hangfire
{
    public static class Helper
    {
        public static IApplicationBuilder UseHangfireDashboard(this IApplicationBuilder app, IConfiguration configuration)
        {
            if (configuration.GetValue("HangfireDashboard:Enable", true) == false) return app;

            var dashboardPath = configuration.GetValue("HangfireDashboard:Path", "/hangfire");
            return app.UseHangfireDashboard(dashboardPath, new DashboardOptions
            {
                DarkModeEnabled = false,
                DisplayNameFunc = JobDisplayName,
                Authorization = null,
            });
        }

        public static string JobDisplayName(DashboardContext db, Job job)
        {
            if (job.Method.DeclaringType?.IsAssignableTo(typeof(HangfireExternalCaller)) == true
                && job.Args[2] is string externalName)
                return externalName;

            return $"{job.Method.DeclaringType?.Name}.{job.Method.Name}";
        }
    }
}
