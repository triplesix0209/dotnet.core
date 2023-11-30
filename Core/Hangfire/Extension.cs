using System.Linq.Expressions;
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
        public static void AddOrUpdateExternal<T>(this IRecurringJobManager recurringJobManager, string recurringJobId, string queue, Expression<Func<T, Task>> methodCall, string cronExpression)
        {
            var serviceTypeName = typeof(T).AssemblyQualifiedName!;
            var method = ((MethodCallExpression)methodCall.Body).Method;
            var arguments = ((MethodCallExpression)methodCall.Body).Arguments
                .Select(argument => Expression.Lambda(argument).Compile().DynamicInvoke()?.ToJson())
                .ToArray();

            recurringJobManager.AddOrUpdate<IHangfireExternalService>(recurringJobId, queue, service => service.Run(serviceTypeName, method.Name, arguments), cronExpression);
        }
    }
}