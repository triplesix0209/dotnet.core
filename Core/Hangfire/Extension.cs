using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Hangfire
{
    /// <summary>
    /// Extension.
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// Đăng ký recurring job với server hangfire.
        /// </summary>
        /// <typeparam name="T">Type dùng để chạy method.</typeparam>
        /// <param name="recurringJobManager"><see cref="IRecurringJobManager"/>.</param>
        /// <param name="recurringJobId">Recurring Job Id.</param>
        /// <param name="queue">Job queue.</param>
        /// <param name="methodCall">Method call.</param>
        /// <param name="cronExpression">Cron expression.</param>
        /// <param name="jobDisplayName">Tên hiển thị của job.</param>
        public static void AddOrUpdateExternal<T>(
            this IRecurringJobManager recurringJobManager,
            string recurringJobId,
            string queue,
            Expression<Func<T, Task>> methodCall,
            string cronExpression,
            string? jobDisplayName = null)
        {
            var serviceTypeName = typeof(T).AssemblyQualifiedName!;
            var method = ((MethodCallExpression)methodCall.Body).Method;
            var arguments = ((MethodCallExpression)methodCall.Body).Arguments
                .Select(argument => Expression.Lambda(argument).Compile().DynamicInvoke()?.ToJson())
                .ToArray();

            jobDisplayName ??= method.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
            var assemblyName = typeof(T).Assembly.GetName().Name!;
            if (jobDisplayName.IsNullOrEmpty())
                jobDisplayName = $"{assemblyName}.{method.Name}";
            else
                jobDisplayName = $"[{assemblyName}] {jobDisplayName}";

            recurringJobManager.AddOrUpdate<HangfireExternalCaller>(
                recurringJobId, queue, service => service.Run(jobDisplayName, serviceTypeName, method.Name, arguments), cronExpression);
        }

        /// <summary>
        /// Khởi chạy hangfire.
        /// </summary>
        /// <param name="serviceProvider"><see cref="IServiceProvider"/>.</param>
        public static void StartHangfire(this IServiceProvider serviceProvider)
        {
            serviceProvider.GetRequiredService<HangfireBaseStartup>().Setup();
        }
    }
}