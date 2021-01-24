using EasyNetQ;
using System;
using System.Threading;

namespace DotnetCore.Sender
{
    class Program
    {
        readonly static IBus bus = RabbitHutch.CreateBus("host=localhost;virtualHost=CUSTOM_VHOST;username=rabbitmq_adm1n;password=Admin@#789;timeout=10");
        static void Main(string[] args)
        {
            try
            {
                for (int i = 0; i < 50; i++)
                {
                    int remainder = i % 5;
                    if (remainder < 3 && i % 10 != 0) // skip multiplication of 10
                    {
                        string message = $"Hello World {i}!";
                        Console.WriteLine($"Sending message: {message}");
                        bus.SendReceive.SendAsync("test.communication.queue", $"Hello World {i}!");
                        Thread.Sleep(1000 * remainder); // randomly sleep the thread to slow the process
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.ReadLine(); // to keep running the console app
        }
    }
}
