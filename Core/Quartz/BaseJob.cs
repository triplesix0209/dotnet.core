using Quartz;

namespace TripleSix.Core.Quartz
{
    /// <summary>
    /// Base job.
    /// </summary>
    public abstract class BaseJob : IJob
    {
        private string JobName
        {
            get
            {
                var name = GetType().Name;
                if (name.EndsWith("Job") && name.Length > "Job".Length)
                    name = name[..^"Job".Length];

                return name;
            }
        }

        /// <summary>
        /// Job builder.
        /// </summary>
        /// <param name="builder"><see cref="JobBuilder"/>.</param>
        /// <returns><see cref="JobBuilder"/>.</returns>
        public virtual JobBuilder JobBuilder(JobBuilder builder)
        {
            return builder.WithIdentity(JobName + "Job");
        }

        /// <summary>
        /// Trigger builder.
        /// </summary>
        /// <param name="builder"><see cref="TriggerBuilder"/>.</param>
        /// <returns><see cref="TriggerBuilder"/>.</returns>
        public virtual TriggerBuilder TriggerBuilder(TriggerBuilder builder)
        {
            return builder.WithIdentity(JobName + "Tigger");
        }

        /// <inheritdoc/>
        public abstract Task Execute(IJobExecutionContext context);
    }
}
