namespace TestConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var sampleListener = new SampleListener();

            sampleListener.Listening();

            var client = new SampleClient();
            client.SendWebRequest("https://docs.microsoft.com/dotnet/core/diagnostics/");

            Console.ReadLine();
        }
    }
}