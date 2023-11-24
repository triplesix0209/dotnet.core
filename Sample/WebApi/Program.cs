namespace Sample.WebApi
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            (await Startup.BuildApp(args)).Run();
        }
    }
}