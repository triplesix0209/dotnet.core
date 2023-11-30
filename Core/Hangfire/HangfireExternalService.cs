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
        public Task Run(string serviceTypeName, string methodName)
        {
            var x = Type.GetType(serviceTypeName);
            return Task.CompletedTask;
            //var service = ServiceProvider.GetService(serviceType)
            //    ?? throw new Exception("Cannot find target service");
            //var method = serviceType.GetMethod(methodName)
            //    ?? throw new Exception("Cannot find target method");

            //var result = method.Invoke(service, null) as Task;
            //if (result == null) return Task.CompletedTask;
            //return result!.WaitAsync(CancellationToken.None);
        }
    }
}
