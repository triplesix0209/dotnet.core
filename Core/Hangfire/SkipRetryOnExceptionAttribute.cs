using Hangfire.Common;
using Hangfire.States;

namespace TripleSix.Core.Hangfire
{
    /// <summary>
    /// An attribute that can be used to exclude a job from automatic retry.
    /// </summary>
    public class SkipRetryOnExceptionAttribute : JobFilterAttribute, IElectStateFilter
    {
        private readonly Type[] _exceptionsToSkip;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkipRetryOnExceptionAttribute"/> class.
        /// </summary>
        /// <param name="exceptionsToSkip">An array of exception types that should be excluded from automatic retry.</param>
        public SkipRetryOnExceptionAttribute(params Type[] exceptionsToSkip)
        {
            _exceptionsToSkip = exceptionsToSkip;
            Order = 1;
        }

        /// <inheritdoc/>
        public void OnStateElection(ElectStateContext context)
        {
            if (context.CandidateState is FailedState failedState && failedState.Exception != null)
            {
                var exceptionType = failedState.Exception.GetType();

                if (_exceptionsToSkip.Any(type => type.IsAssignableFrom(exceptionType)))
                    context.SetJobParameter("RetryCount", 9999);
            }
        }
    }
}