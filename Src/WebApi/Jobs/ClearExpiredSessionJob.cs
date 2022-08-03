namespace Sample.WebApi.Jobs
{
    public class ClearExpiredSessionJob : BaseJob
    {
        public IIdentityService IdentityService { get; set; }

        public override TriggerBuilder TriggerBuilder(TriggerBuilder builder)
            => base.TriggerBuilder(builder)
            .WithSimpleSchedule(x => x.RepeatForever().WithIntervalInSeconds(1))
            .StartNow();

        public override async Task Execute(IJobExecutionContext context)
        {
            await IdentityService.ClearAllExpiredSession();
        }
    }
}
