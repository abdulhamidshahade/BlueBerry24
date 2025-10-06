namespace BlueBerry24.Application.Dtos.OrderDtos
{
    public class MarkOrderPaidRequest
    {
        public int PaymentTransactionId { get; set; }
        public string PaymentProvider { get; set; } = string.Empty;
    }

}
