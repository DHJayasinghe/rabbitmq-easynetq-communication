namespace EventBusMessages
{
    public sealed class PaymentValidityCheckRequestMessage
    {
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentType { get; set; }
    }
}
