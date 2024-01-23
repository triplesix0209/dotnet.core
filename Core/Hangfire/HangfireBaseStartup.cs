using Hangfire;

namespace TripleSix.Core.Hangfire
{
    /// <summary>
    /// Hangfire Base Startup.
    /// </summary>
    public abstract class HangfireBaseStartup
    {
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
    }
}
