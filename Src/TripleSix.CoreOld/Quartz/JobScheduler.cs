using System;
using System.Linq;
using Autofac;
using Quartz;

namespace TripleSix.CoreOld.Quartz
{
    public class JobScheduler
    {
        private readonly IScheduler _scheduler;

        public JobScheduler(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public IComponentContext Container { get; set; }

        public void Start(string[] excludeAssemblyNames = null)
        {
            var jobTypes = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => excludeAssemblyNames == null || !excludeAssemblyNames.Contains(assembly.GetName().Name))
                .SelectMany(assembly => assembly.GetTypes()
                .Where(t => t.IsPublic)
                .Where(t => !t.IsAbstract)
                .Where(t => t.IsAssignableTo<BaseJob>()));

            foreach (var jobType in jobTypes)
            {
                var instance = (BaseJob)Activator.CreateInstance(jobType);
                var job = instance.JobBuilder(JobBuilder.Create(jobType)).Build();
                var trigger = instance.TriggerBuilder(TriggerBuilder.Create()).Build();
                _scheduler.ScheduleJob(job, trigger);
            }

            _scheduler.Start();
        }
    }
}
