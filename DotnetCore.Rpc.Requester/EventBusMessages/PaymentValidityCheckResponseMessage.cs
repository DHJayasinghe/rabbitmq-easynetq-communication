using EasyNetQ;

namespace EventBusMessages
{
    [Queue(queueName: "payment.validity.check.queue")]
    public sealed class PaymentValidityCheckResponseMessage
    {
        public bool Valid { get; set; }
        public string Message { get; set; }
    }
}
