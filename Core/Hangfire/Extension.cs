#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

using System.Linq.Expressions;
using Hangfire;
using Hangfire.Common;
using Hangfire.Dashboard;
using Hangfire.Dashboard.BasicAuthorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TripleSix.Core.Appsettings;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Hangfire
{
    /// <summary>
    /// Extension.
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// Đăng ký fire-and-forget job với server hangfire.
        /// </summary>
        /// <typeparam name="T">Type dùng để chạy method.</typeparam>
        /// <param name="backgroundJobClient"><see cref="IBackgroundJobClient"/>.</param>
        /// <param name="methodCall">Method call.</param>
        /// <param name="jobOption"><see cref="JobOption"/>.</param>
        public static void EnqueueExternal<T>(
            this IBackgroundJobClient backgroundJobClient,
            Expression<Func<T, Task>> methodCall,
            JobOption? jobOption = null)
        {
            var serviceTypeName = typeof(T).AssemblyQualifiedName!;
            var method = ((MethodCallExpression)methodCall.Body).Method;
            var arguments = ((MethodCallExpression)methodCall.Body).Arguments
                .Select(argument => Expression.Lambda(argument).Compile().DynamicInvoke()?.ToJson())
                .ToArray();

            jobOption ??= new JobOption();
            var queue = jobOption.GetMethodQueue(method);
            var displayName = jobOption.GetMethodDisplayName(method);

            backgroundJobClient.Enqueue<HangfireExternalCaller>(
                queue, service => service.Run(null, null, displayName, serviceTypeName, method.Name, arguments));
        }

        /// <summary>
        /// Đăng ký delay job với server hangfire.
        /// </summary>
        /// <typeparam name="T">Type dùng để chạy method.</typeparam>
        /// <param name="backgroundJobClient"><see cref="IBackgroundJobClient"/>.</param>
        /// <param name="methodCall">Method call.</param>
        /// <param name="delay">Thời gian chờ.</param>
        /// <param name="jobOption"><see cref="JobOption"/>.</param>
        public static void ScheduleExternal<T>(
            this IBackgroundJobClient backgroundJobClient,
            Expression<Func<T, Task>> methodCall,
            TimeSpan delay,
            JobOption? jobOption = null)
        {
            var serviceTypeName = typeof(T).AssemblyQualifiedName!;
            var method = ((MethodCallExpression)methodCall.Body).Method;
            var arguments = ((MethodCallExpression)methodCall.Body).Arguments
                .Select(argument => Expression.Lambda(argument).Compile().DynamicInvoke()?.ToJson())
                .ToArray();

            jobOption ??= new JobOption();
            var queue = jobOption.GetMethodQueue(method);
            var displayName = jobOption.GetMethodDisplayName(method);

            backgroundJobClient.Schedule<HangfireExternalCaller>(
                queue, service => service.Run(null, null, displayName, serviceTypeName, method.Name, arguments), delay);
        }

        /// <summary>
        /// Đăng ký continuation job với server hangfire.
        /// </summary>
        /// <typeparam name="T">Type dùng để chạy method.</typeparam>
        /// <param name="backgroundJobClient"><see cref="IBackgroundJobClient"/>.</param>
        /// <param name="parentId">Id job cha, sẽ chạy job đăng ký nếu job cha chạy xong.</param>
        /// <param name="methodCall">Method call.</param>
        /// <param name="jobOption"><see cref="JobOption"/>.</param>
        public static void ContinueJobWithExternal<T>(
            this IBackgroundJobClient backgroundJobClient,
            string parentId,
            Expression<Func<T, Task>> methodCall,
            JobOption? jobOption = null)
        {
            var serviceTypeName = typeof(T).AssemblyQualifiedName!;
            var method = ((MethodCallExpression)methodCall.Body).Method;
            var arguments = ((MethodCallExpression)methodCall.Body).Arguments
                .Select(argument => Expression.Lambda(argument).Compile().DynamicInvoke()?.ToJson())
                .ToArray();

            jobOption ??= new JobOption();
            var queue = jobOption.GetMethodQueue(method);
            var displayName = jobOption.GetMethodDisplayName(method);

            backgroundJobClient.ContinueJobWith<HangfireExternalCaller>(
                parentId, queue, service => service.Run(null, null, displayName, serviceTypeName, method.Name, arguments));
        }

        /// <summary>
        /// Đăng ký recurring job với server hangfire.
        /// </summary>
        /// <typeparam name="T">Type dùng để chạy method.</typeparam>
        /// <param name="recurringJobManager"><see cref="IRecurringJobManager"/>.</param>
        /// <param name="recurringJobId">Recurring Job Id.</param>
        /// <param name="methodCall">Method call.</param>
        /// <param name="cronExpression">Cron expression.</param>
        /// <param name="jobOption"><see cref="JobOption"/>.</param>
        public static void AddOrUpdateExternal<T>(
            this IRecurringJobManager recurringJobManager,
            string recurringJobId,
            Expression<Func<T, Task>> methodCall,
            string cronExpression,
            JobOption? jobOption = null)
        {
            var serviceTypeName = typeof(T).AssemblyQualifiedName!;
            var method = ((MethodCallExpression)methodCall.Body).Method;
            var arguments = ((MethodCallExpression)methodCall.Body).Arguments
                .Select(argument => Expression.Lambda(argument).Compile().DynamicInvoke()?.ToJson())
                .ToArray();

            jobOption ??= new JobOption();
            var queue = jobOption.GetMethodQueue(method);
            var displayName = jobOption.GetMethodDisplayName(method);

            recurringJobManager.AddOrUpdate<HangfireExternalCaller>(
                recurringJobId, queue, service => service.Run(null, null, displayName, serviceTypeName, method.Name, arguments), cronExpression);
        }

        /// <summary>
        /// Cấu hình Hangfire Dashboard.
        /// </summary>
        /// <param name="app"><see cref="IApplicationBuilder"/>.</param>
        /// <param name="setting"><see cref="HangfireAppsetting"/>.</param>
        /// <param name="setup">Hàm tinh chỉnh option.</param>
        /// <returns><see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder UseHangfireDashboard(this IApplicationBuilder app, HangfireAppsetting setting, Action<DashboardOptions>? setup = null)
        {
            if (setting.DashboardEnable == false) return app;

            var option = new DashboardOptions
            {
                DarkModeEnabled = false,
                DisplayNameFunc = JobDisplayName,
            };

            if (setting.DashboardUsername.IsNotNullOrEmpty() && setting.DashboardPassword.IsNotNullOrEmpty())
            {
                option.Authorization = new[]
                {
                    new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
                    {
                        RequireSsl = false,
                        SslRedirect = false,
                        LoginCaseSensitive = true,
                        Users = new List<BasicAuthAuthorizationUser>()
                        {
                            new BasicAuthAuthorizationUser { Login = setting.DashboardUsername, PasswordClear = setting.DashboardPassword },
                        },
                    }),
                };
            }

            setup?.Invoke(option);
            return app.UseHangfireDashboard(setting.DashboardPath, option);
        }

        /// <summary>
        /// Cấu hình Hangfire Dashboard.
        /// </summary>
        /// <param name="app"><see cref="IApplicationBuilder"/>.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        /// <param name="setup">Hàm tinh chỉnh option.</param>
        /// <returns><see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder UseHangfireDashboard(this IApplicationBuilder app, IConfiguration configuration, Action<DashboardOptions>? setup = null)
        {
            return UseHangfireDashboard(app, new HangfireAppsetting(configuration), setup);
        }

        /// <summary>
        /// Hàm hiển thị tên Job.
        /// </summary>
        /// <param name="db"><see cref="DashboardContext"/>.</param>
        /// <param name="job"><see cref="Job"/>.</param>
        /// <returns>Tên job.</returns>
        public static string JobDisplayName(DashboardContext db, Job job)
        {
            if (job.Method.DeclaringType?.IsAssignableTo(typeof(HangfireExternalCaller)) == true
                && job.Args[2] is string externalName)
                return externalName;

            return $"{job.Method.DeclaringType?.Name}.{job.Method.Name}";
        }

        /// <summary>
        /// Khởi chạy hangfire.
        /// </summary>
        /// <param name="serviceProvider"><see cref="IServiceProvider"/>.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task StartHangfire(this IServiceProvider serviceProvider)
        {
            await serviceProvider.GetRequiredService<HangfireBaseStartup>().Setup();
        }
    }
}