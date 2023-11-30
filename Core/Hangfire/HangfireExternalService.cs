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
        public async Task Run(string serviceTypeName, string methodName, params string?[]? arguments)
        {
            var serviceType = Type.GetType(serviceTypeName)
                ?? throw new Exception("Cannot find target service");
            var service = ServiceProvider.GetService(serviceType)
                ?? throw new Exception("Cannot find target service");
            var method = serviceType.GetMethod(methodName)
                ?? throw new Exception("Cannot find target method");

            var result = method.Invoke(service, new object[] { }) as Task;
            await result!.WaitAsync(CancellationToken.None);
        }
    }
}
