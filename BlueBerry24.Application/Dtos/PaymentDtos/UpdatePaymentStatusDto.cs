using BlueBerry24.Domain.Constants;

namespace BlueBerry24.API.Controllers
{
    public class UpdatePaymentStatusDto
    {
        public PaymentStatus Status { get; set; }
        public string? Notes { get; set; }
    }
}
