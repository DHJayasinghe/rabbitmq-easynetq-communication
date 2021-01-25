using EasyNetQ;
using EventBusMessages;
using System;
using System.Threading;

namespace DotnetCore.Sender
{
    class Program
    {
        private readonly static IBus bus = RabbitHutch.CreateBus(
            connectionString: "host=localhost;virtualHost=CUSTOM_VHOST;username=rabbitmq_adm1n;password=Admin@#789;timeout=10",
            registerServices: s =>
            {
                s.Register<ITypeNameSerializer, EventBusTypeNameSerializer>();
            });

        static void Main(string[] args)
        {
            try
            {
                for (int i = 0; i < 50; i++)
                {
                    int remainder = i % 5;
                    if (remainder < 3 && i % 10 != 0) // intentionally skip multiplication of 10
                    {
                        //var message = $"Hello World {i}!"; // plain text message
                        var message = new PlaceOrderRequestMessage
                        {
                            Name = $"Order #: {i}",
                            DateTimeCreated = DateTime.Now,
                            OrderItems = new System.Collections.Generic.List<OrderItemDTO>{ new OrderItemDTO
                                {
                                    Id = i,
                                    ItemQty = i*5,
                                    Total = i*5 * 750.5
                                }
                            }
                        };

                        Console.WriteLine($"Sending message: {message.Name}");
                        //bus.SendReceive.SendAsync("test.communication.queue", message);

                        bus.SendReceive.SendAsync("test.communication.queue", message);
                        Thread.Sleep(1000 * remainder); // intentionally sleep the thread to slow down the process
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
