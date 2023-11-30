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
        /// <param name="serviceTypeName">Type service.</param>
        /// <param name="methodName">Method name.</param>
        /// <param name="arguments">Method arguments.</param>
        /// <returns>Task.</returns>
        Task Run(string serviceTypeName, string methodName, params string?[]? arguments);
    }
}
