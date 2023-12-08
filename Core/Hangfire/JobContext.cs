using Hangfire;
using Hangfire.Server;

namespace TripleSix.Core.Hangfire
{
    /// <summary>
    /// Job context.
    /// </summary>
    public class JobContext
    {
        /// <summary>
        /// Perform context.
        /// </summary>
        public PerformContext PerformContext { get; set; }

        /// <summary>
        /// Cancellation token.
        /// </summary>
        public IJobCancellationToken CancellationToken { get; set; }
    }
}
