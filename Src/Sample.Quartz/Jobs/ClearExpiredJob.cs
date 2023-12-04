using System.Threading.Tasks;
using Quartz;
using Sample.Common;
using Sample.Middle.Services;
using TripleSix.CoreOld.Quartz;

namespace Sample.Quartz.Jobs
{
    public class ClearExpiredJob : BaseJob
    {
        public IIdentityService IdentityService { get; set; }

        public IAccountService AccountService { get; set; }

        public override TriggerBuilder TriggerBuilder(TriggerBuilder builder)
            => base.TriggerBuilder(builder)
            .WithSimpleSchedule(x => x.RepeatForever().WithIntervalInSeconds(1))
            .StartNow();

        public override async Task Execute(IJobExecutionContext context)
        {
            var identity = new Identity();

            await IdentityService.ClearExpiredSession(identity);
            await AccountService.ClearVerifyExpired(identity);
        }
    }
}
