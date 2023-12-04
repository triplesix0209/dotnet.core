using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using TripleSix.Core.Helpers;
using TripleSix.Core.JsonSerializers;

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
        /// <param name="queue">Job queue.</param>
        /// <param name="methodCall">Method call.</param>
        /// <param name="jobDisplayName">Tên hiển thị của job.</param>
        public static void EnqueueExternal<T>(
            this IBackgroundJobClient backgroundJobClient,
            string queue,
            Expression<Func<T, Task>> methodCall,
            string? jobDisplayName = null)
        {
            var serviceTypeName = typeof(T).AssemblyQualifiedName!;
            var method = ((MethodCallExpression)methodCall.Body).Method;
            var arguments = ((MethodCallExpression)methodCall.Body).Arguments
                .Select(argument => JsonHelper.SerializeObject(Expression.Lambda(argument).Compile().DynamicInvoke()))
                .ToArray();

            jobDisplayName ??= method.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
            var assemblyName = typeof(T).Assembly.GetName().Name!;
            if (jobDisplayName.IsNullOrEmpty())
                jobDisplayName = $"{assemblyName}.{method.Name}";
            else
                jobDisplayName = $"[{assemblyName}] {jobDisplayName}";

            backgroundJobClient.Enqueue<HangfireExternalCaller>(
                queue, service => service.Run(jobDisplayName, serviceTypeName, method.Name, arguments));
        }

        /// <summary>
        /// Đăng ký delay job với server hangfire.
        /// </summary>
        /// <typeparam name="T">Type dùng để chạy method.</typeparam>
        /// <param name="backgroundJobClient"><see cref="IBackgroundJobClient"/>.</param>
        /// <param name="queue">Job queue.</param>
        /// <param name="methodCall">Method call.</param>
        /// <param name="delay">Thời gian chờ.</param>
        /// <param name="jobDisplayName">Tên hiển thị của job.</param>
        public static void ScheduleExternal<T>(
            this IBackgroundJobClient backgroundJobClient,
            string queue,
            Expression<Func<T, Task>> methodCall,
            TimeSpan delay,
            string? jobDisplayName = null)
        {
            var serviceTypeName = typeof(T).AssemblyQualifiedName!;
            var method = ((MethodCallExpression)methodCall.Body).Method;
            var arguments = ((MethodCallExpression)methodCall.Body).Arguments
                .Select(argument => JsonHelper.SerializeObject(Expression.Lambda(argument).Compile().DynamicInvoke()))
                .ToArray();

            jobDisplayName ??= method.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
            var assemblyName = typeof(T).Assembly.GetName().Name!;
            if (jobDisplayName.IsNullOrEmpty())
                jobDisplayName = $"{assemblyName}.{method.Name}";
            else
                jobDisplayName = $"[{assemblyName}] {jobDisplayName}";

            backgroundJobClient.Schedule<HangfireExternalCaller>(
                queue, service => service.Run(jobDisplayName, serviceTypeName, method.Name, arguments), delay);
        }

        /// <summary>
        /// Đăng ký continuation job với server hangfire.
        /// </summary>
        /// <typeparam name="T">Type dùng để chạy method.</typeparam>
        /// <param name="backgroundJobClient"><see cref="IBackgroundJobClient"/>.</param>
        /// <param name="parentId">Id job cha, sẽ chạy job đăng ký nếu job cha chạy xong.</param>
        /// <param name="queue">Job queue.</param>
        /// <param name="methodCall">Method call.</param>
        /// <param name="jobDisplayName">Tên hiển thị của job.</param>
        public static void ContinueJobWithExternal<T>(
            this IBackgroundJobClient backgroundJobClient,
            string parentId,
            string queue,
            Expression<Func<T, Task>> methodCall,
            string? jobDisplayName = null)
        {
            var serviceTypeName = typeof(T).AssemblyQualifiedName!;
            var method = ((MethodCallExpression)methodCall.Body).Method;
            var arguments = ((MethodCallExpression)methodCall.Body).Arguments
                .Select(argument => JsonHelper.SerializeObject(Expression.Lambda(argument).Compile().DynamicInvoke()))
                .ToArray();

            jobDisplayName ??= method.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
            var assemblyName = typeof(T).Assembly.GetName().Name!;
            if (jobDisplayName.IsNullOrEmpty())
                jobDisplayName = $"{assemblyName}.{method.Name}";
            else
                jobDisplayName = $"[{assemblyName}] {jobDisplayName}";

            backgroundJobClient.ContinueJobWith<HangfireExternalCaller>(
                parentId, queue, service => service.Run(jobDisplayName, serviceTypeName, method.Name, arguments));
        }

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
                .Select(argument => JsonHelper.SerializeObject(Expression.Lambda(argument).Compile().DynamicInvoke()))
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