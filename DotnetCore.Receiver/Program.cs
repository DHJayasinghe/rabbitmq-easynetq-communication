using EasyNetQ;
using EventBusMessages;
using Newtonsoft.Json;
using System;

namespace DotnetCore.Receiver
{
    class Program
    {
        readonly static IBus bus = RabbitHutch.CreateBus(
            connectionString: "host=localhost;virtualHost=CUSTOM_VHOST;username=rabbitmq_adm1n;password=Admin@#789;timeout=10",
            registerServices: s =>
              {
                  s.Register<ITypeNameSerializer, EventBusTypeNameSerializer>();
              });
        static void Main(string[] args)
        {
            try
            {
                bus.SendReceive.ReceiveAsync<PlaceOrderRequestMessage>("test.communication.queue", msg =>
                {
                    Console.WriteLine($"Received message: {JsonConvert.SerializeObject(msg)}");
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
