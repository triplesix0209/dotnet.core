using TripleSix.Core.Helpers;

namespace TripleSix.Core.Hangfire
{
    /// <summary>
    /// Hangfire external service.
    /// </summary>
    public class HangfireExternalService : IHangfireExternalService
    {
        /// <summary>
        /// Service Provider.
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }

        /// <inheritdoc/>
        public async Task Run(string jobDisplayName, string serviceTypeName, string methodName, params string?[]? arguments)
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
                    return value.ToObject(parameterTypes[index].ParameterType);
                }).ToArray();

            var result = method.Invoke(service, parameters) as Task;
            await result!.WaitAsync(CancellationToken.None);
        }
    }
}
