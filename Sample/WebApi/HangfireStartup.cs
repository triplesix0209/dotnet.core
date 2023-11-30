using Sample.Application.Services;
using TripleSix.Core.Hangfire;

namespace Sample.WebApi
{
    public class HangfireStartup : HangfireBaseStartup
    {
        public override void Setup()
        {
            RecurringJob.AddOrUpdateExternal<ISiteService>("Sample", "sample", service => service.RunJob("Test", 1, DateTime.UtcNow), "* * * * *");
        }
    }
}
