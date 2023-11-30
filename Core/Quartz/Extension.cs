using Microsoft.Extensions.DependencyInjection;

namespace TripleSix.Core.Quartz
{
    /// <summary>
    /// Extension.
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// Khởi chạy quartz.
        /// </summary>
        /// <param name="serviceProvider"><see cref="IServiceProvider"/>.</param>
        public static void StartQuartz(this IServiceProvider serviceProvider)
        {
            serviceProvider.GetRequiredService<JobScheduler>().Start();
        }
    }
}