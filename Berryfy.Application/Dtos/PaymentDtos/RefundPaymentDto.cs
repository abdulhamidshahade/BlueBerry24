namespace Berryfy.Application.Dtos.PaymentDtos
{
    public class RefundPaymentDto
    {
        public decimal? RefundAmount { get; set; }
        public string? Reason { get; set; }
    }
}
