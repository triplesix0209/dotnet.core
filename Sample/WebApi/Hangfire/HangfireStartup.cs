using Hangfire;

namespace Sample.WebApi.Hangfire
{
    public class HangfireStartup : HangfireBaseStartup
    {
        public override async Task Setup()
        {
            await InitServer();
        }
    }
}
