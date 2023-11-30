namespace TripleSix.Core.Hangfire
{
    /// <summary>
    /// Hangfire external service.
    /// </summary>
    public interface IHangfireExternalService
    {
        /// <summary>
        /// Chạy method.
        /// </summary>
        /// <param name="serviceType">Type service.</param>
        /// <param name="methodName">Method name.</param>
        /// <returns>Task.</returns>
        Task Run(string serviceTypeName, string methodName);
    }
}
