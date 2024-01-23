namespace Sample.WebApi.Hangfire
{
    public class HangfireStartup : HangfireBaseStartup
    {
        public override Task Setup()
        {
            return Task.CompletedTask;
        }
    }
}
