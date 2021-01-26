using EasyNetQ;
using EventBusMessages;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace DotnetCore.Rpc.Responder
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

        static async Task Main(string[] args)
        {
            try
            {
                await bus.Rpc.RespondAsync<PaymentValidityCheckRequestMessage, PaymentValidityCheckResponseMessage>(request =>
                {
                    Console.WriteLine($"Received request: : {JsonConvert.SerializeObject(request)}");
                    // some example validation logic
                    bool valid = request.CustomerId > 0 && request.Amount > 0 && request.PaymentType == "CASH";
                    return new PaymentValidityCheckResponseMessage
                    {
                        Valid = valid,
                        Message = valid ? "Valid payment" : "Invalid payment"
                    };
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
