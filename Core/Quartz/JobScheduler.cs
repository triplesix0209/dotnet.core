using Autofac;
using Quartz;

namespace TripleSix.Core.Quartz
{
    /// <summary>
    /// Job scheduler.
    /// </summary>
    public class JobScheduler
    {
        private readonly IScheduler _scheduler;

        /// <summary>
        /// Job scheduler.
        /// </summary>
        /// <param name="scheduler"><see cref="IScheduler"/>.</param>
        public JobScheduler(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        /// <summary>
        /// Component context.
        /// </summary>
        public IComponentContext Container { get; set; }

        /// <summary>
        /// Start scheduler.
        /// </summary>
        public void Start()
        {
            var jobTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes()
                .Where(t => t.IsPublic)
                .Where(t => !t.IsAbstract)
                .Where(t => t.IsAssignableTo<BaseJob>()));

            foreach (var jobType in jobTypes)
            {
                var instance = Activator.CreateInstance(jobType) as BaseJob;
                if (instance == null) continue;

                var job = instance.JobBuilder(JobBuilder.Create(jobType)).Build();
                var trigger = instance.TriggerBuilder(TriggerBuilder.Create()).Build();
                _scheduler.ScheduleJob(job, trigger);
            }

            _scheduler.Start();
        }
    }
}
