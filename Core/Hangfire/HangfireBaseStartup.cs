using Hangfire;
using TripleSix.Core.Appsettings;

namespace TripleSix.Core.Hangfire
{
    /// <summary>
    /// Hangfire Base Startup.
    /// </summary>
    public abstract class HangfireBaseStartup
    {
        /// <summary>
        /// <see cref="HangfireAppsetting"/>.
        /// </summary>
        public HangfireAppsetting Setting { get; set; }

        /// <summary>
        /// Hangfire Worker Manager.
        /// </summary>
        public IHangfireServerManager ServerManager { get; set; }

        /// <summary>
        /// Recurring Job Manager.
        /// </summary>
        public IRecurringJobManager RecurringJob { get; set; }

        /// <summary>
        /// Background Job Client.
        /// </summary>
        public IBackgroundJobClient BackgroundJob { get; set; }

        /// <summary>
        /// Perform setup.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public abstract Task Setup();

        /// <summary>
        /// Run Hangfire Servers.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task Run()
        {
            if (!Setting.Enable) return;
            if (Setting.StartupWorker) ServerManager.Start();
            await Setup();
        }
    }
}
