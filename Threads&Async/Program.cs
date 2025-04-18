namespace Threads_Async
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Race race = new Race();
            race.Racing().Wait();
        }
    }
}
