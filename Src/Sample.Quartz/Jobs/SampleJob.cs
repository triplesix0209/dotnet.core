using System.Threading.Tasks;
using Quartz;
using TripleSix.Core.Quartz;

namespace Sample.Quartz.Jobs
{
    public class SampleJob : BaseJob
    {
        public override TriggerBuilder TriggerBuilder(TriggerBuilder builder)
            => base.TriggerBuilder(builder)
            .WithSimpleSchedule(x => x.RepeatForever().WithIntervalInSeconds(1))
            .StartNow();

        public override Task Execute(IJobExecutionContext context)
        {
            return Task.CompletedTask;
        }
    }
}
