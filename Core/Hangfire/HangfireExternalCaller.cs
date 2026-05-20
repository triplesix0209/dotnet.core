using System.Reflection;
using Hangfire;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using TripleSix.Core.DataContext;
using TripleSix.Core.Helpers;
using TripleSix.Core.WebApi;

namespace TripleSix.Core.Hangfire
{
    /// <summary>
    /// Hangfire External Caller.
    /// </summary>
    public class HangfireExternalCaller
    {
        /// <summary>
        /// Service Provider.
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// Chạy method.
        /// </summary>
        /// <param name="performContext"><see cref="PerformContext"/>.</param>
        /// <param name="cancellationToken"><see cref="IJobCancellationToken"/>.</param>
        /// <param name="jobDisplayName">Tên hiển thị của job.</param>
        /// <param name="serviceTypeName">Type service.</param>
        /// <param name="methodName">Method name.</param>
        /// <param name="arguments">Method arguments.</param>
        /// <returns>Task.</returns>
        public async Task Run(PerformContext performContext, IJobCancellationToken cancellationToken, string jobDisplayName, string serviceTypeName, string methodName, params string?[]? arguments)
        {
            var serviceType = Type.GetType(serviceTypeName)
                ?? throw new Exception("Cannot find target service");
            var service = ServiceProvider.GetService(serviceType)
                ?? throw new Exception("Cannot find target service");
            var method = serviceType.GetMethod(methodName)
                ?? serviceType.GetInterfaces().Select(x => x.GetMethod(methodName)).FirstOrDefault(x => x != null)
                ?? throw new Exception("Cannot find target method");

            var parameterTypes = method.GetParameters();
            var parameters = arguments?.Select((value, index) =>
            {
                if (parameterTypes[index].ParameterType == typeof(JobContext))
                    return new JobContext { PerformContext = performContext, CancellationToken = cancellationToken };

                if (value == null) return null;
                return value.ToObject(parameterTypes[index].ParameterType);
            });

            var transactionalAttr = method.GetCustomAttribute<Transactional>(true)
                ?? serviceType.GetCustomAttribute<Transactional>(true);

            if (transactionalAttr != null)
            {
                var dbContexts = transactionalAttr.DbContextTypes
                    .Select(type => (IDbDataContext)ServiceProvider.GetRequiredService(type))
                    .Distinct()
                    .ToList();
                var transactions = new List<IDbContextTransaction>();
                try
                {
                    foreach (var db in dbContexts)
                        transactions.Add(await db.BeginTransactionAsync());

                    var result = method.Invoke(service, [.. parameters!]) as Task;
                    await result!.WaitAsync(CancellationToken.None);

                    foreach (var transaction in transactions)
                        await transaction.CommitAsync();
                }
                catch
                {
                    foreach (var transaction in transactions)
                        await transaction.RollbackAsync();

                    throw;
                }
                finally
                {
                    foreach (var transaction in transactions)
                        await transaction.DisposeAsync();
                }
            }
            else
            {
                var result = method.Invoke(service, [.. parameters!]) as Task;
                await result!.WaitAsync(CancellationToken.None);
            }
        }
    }
}
