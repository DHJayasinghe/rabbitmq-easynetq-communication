using EasyNetQ;

namespace EventBusMessages
{
    [Queue(queueName: "payment.validity.check.queue")]
    public sealed class PaymentValidityCheckRequestMessage
    {
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentType { get; set; }
    }
}
