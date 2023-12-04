using System;
using System.Linq;
using System.Threading.Tasks;
using TripleSix.Core.Helpers;
using TripleSix.Core.JsonSerializers;

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
        /// <param name="jobDisplayName">Tên hiển thị của job.</param>
        /// <param name="serviceTypeName">Type service.</param>
        /// <param name="methodName">Method name.</param>
        /// <param name="arguments">Method arguments.</param>
        /// <returns>Task.</returns>
        public async Task Run(string jobDisplayName, string serviceTypeName, string methodName, params string[] arguments)
        {
            var serviceType = Type.GetType(serviceTypeName)
                ?? throw new Exception("Cannot find target service");
            var service = ServiceProvider.GetService(serviceType)
                ?? throw new Exception("Cannot find target service");
            var method = serviceType.GetMethod(methodName)
                ?? throw new Exception("Cannot find target method");

            var parameterTypes = method.GetParameters();
            var parameters = arguments?
                .Select((value, index) =>
                {
                    if (value == null) return null;
                    return JsonHelper.ToObject(value, parameterTypes[index].ParameterType);
                }).ToArray();

            var result = method.Invoke(service, parameters) as Task;
            result.Wait();
        }
    }
}
