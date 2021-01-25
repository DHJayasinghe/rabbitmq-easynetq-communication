namespace EventBusMessages
{
    public sealed class PaymentValidityCheckResponseMessage
    {
        public bool Valid { get; set; }
        public string Message { get; set; }
    }
}
