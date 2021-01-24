using EasyNetQ;
using System;

namespace DotnetCore.Receiver
{
    class Program
    {
        readonly static IBus bus = RabbitHutch.CreateBus("host=localhost;virtualHost=CUSTOM_VHOST;username=rabbitmq_adm1n;password=Admin@#789;timeout=10");
        static void Main(string[] args)
        {
            try
            {
                bus.SendReceive.ReceiveAsync<string>("test.communication.queue", msg =>
                {
                    Console.WriteLine($"Received message: {msg}");
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.ReadLine(); // to keep running the console app
        }
    }
}
