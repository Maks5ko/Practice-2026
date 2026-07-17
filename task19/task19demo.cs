using System;
using System.Threading;
using task18;
namespace task19
{
    class task19demo
    {
        static void Main()
        {
            var scheduler = new RoundRobinScheduler();
            var server = new ServerThread(scheduler);

            for (int i = 1; i <= 5; i++)
            {
                server.Enqueue(new TestCommand(i, 3));
            }

            Thread.Sleep(2000);
            
            server.EnqueueHardStop();

            server.Join();
            Console.WriteLine("Программа завершена.");
        }
    }
}
