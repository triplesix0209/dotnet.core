﻿using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using Hangfire;
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

            jobDisplayName ??= method.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? method.Name;
            var assemblyName = typeof(T).Assembly.GetName().Name;
            if (assemblyName.IsNotNullOrEmpty()) jobDisplayName = $"{assemblyName}.{jobDisplayName}";

            recurringJobManager.AddOrUpdate<IHangfireExternalService>(
                recurringJobId, queue, service => service.Run(jobDisplayName, serviceTypeName, method.Name, arguments), cronExpression);
        }
    }
}