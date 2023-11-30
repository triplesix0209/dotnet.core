using Hangfire;
using Sample.Application.Services;
using TripleSix.Core.Hangfire;

namespace DMCLSample.WebApi.Hangfire
{
    public class HangfireStartup
    {
        public IRecurringJobManager RecurringJobManager { get; set; }

        public void Setup()
        {
            RecurringJobManager.AddOrUpdateExternal<ISiteService>("Sample", "sample", service => service.RunJob("Test", 1, DateTime.UtcNow), "* * * * *");
        }
    }
}
