using Sample.Application.Services;
using TripleSix.Core.Hangfire;

namespace Sample.WebApi
{
    public class HangfireStartup : HangfireBaseStartup
    {
        public override void Setup()
        {
            BackgroundJob.EnqueueExternal<ISiteService>(service => service.RunJob(null));
            //RecurringJob.AddOrUpdateExternal<ISiteService>("Sample", service => service.RunJob(), "* * * * *");
        }
    }
}
