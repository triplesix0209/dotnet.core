using Hangfire;
using Hangfire.Server;
using TripleSix.Core.Helpers;

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
                ?? throw new Exception("Cannot find target method");

            var parameterTypes = method.GetParameters();
            var parameters = arguments?.Select((value, index) =>
            {
                if (parameterTypes[index].ParameterType == typeof(JobContext))
                    return new JobContext { PerformContext = performContext, CancellationToken = cancellationToken };

                if (value == null) return null;
                return value.ToObject(parameterTypes[index].ParameterType);
            });

            var result = method.Invoke(service, parameters!.ToArray()) as Task;
            await result!.WaitAsync(CancellationToken.None);
        }
    }
}
