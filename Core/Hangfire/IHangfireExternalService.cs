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
        /// <param name="jobDisplayName">Tên hiển thị của job.</param>
        /// <param name="serviceTypeName">Type service.</param>
        /// <param name="methodName">Method name.</param>
        /// <param name="arguments">Method arguments.</param>
        /// <returns>Task.</returns>
        Task Run(string jobDisplayName, string serviceTypeName, string methodName, params string?[]? arguments);
    }
}
