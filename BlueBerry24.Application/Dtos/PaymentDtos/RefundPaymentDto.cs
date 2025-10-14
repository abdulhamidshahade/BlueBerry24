namespace BlueBerry24.Application.Dtos.PaymentDtos
{
    public class RefundPaymentDto
    {
        public decimal? RefundAmount { get; set; }
        public string? Reason { get; set; }
    }
}
