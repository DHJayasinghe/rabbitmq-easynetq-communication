using EasyNetQ;
using EventBusMessages;
using System;
using System.Threading.Tasks;

namespace DotnetCore.Rpc.Requester
{
    class Program
    {
        private readonly static IBus bus = RabbitHutch.CreateBus(
            connectionString: "host=localhost;virtualHost=CUSTOM_VHOST;username=rabbitmq_adm1n;password=Admin@#789;timeout=10",
            registerServices: s =>
            {
                s.Register<ITypeNameSerializer, EventBusTypeNameSerializer>();
                s.Register<IConventions, EventBusConventions>();
            });

        static void Main(string[] args)
        {
            try
            {
                var task1 = bus.Rpc.RequestAsync<PaymentValidityCheckRequestMessage, PaymentValidityCheckResponseMessage>(new PaymentValidityCheckRequestMessage
                {
                    Amount = 100,
                    CustomerId = 1,
                    PaymentType = "CASH"
                });
                var task2 = bus.Rpc.RequestAsync<PaymentValidityCheckRequestMessage, PaymentValidityCheckResponseMessage>(new PaymentValidityCheckRequestMessage
                {
                    Amount = -500,
                    CustomerId = 1,
                    PaymentType = "CREDIT"
                });
                Task.WaitAll(task1, task2); // wait for all tasks to complete

                Console.WriteLine("Received response: " + task1.Result.Message);
                Console.WriteLine("Received response: " + task2.Result.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.ReadLine(); // to keep running the console app
        }
    }
}
