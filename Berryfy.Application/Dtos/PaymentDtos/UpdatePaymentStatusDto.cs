using Berryfy.Domain.Constants;

namespace Berryfy.API.Controllers
{
    public class UpdatePaymentStatusDto
    {
        public PaymentStatus Status { get; set; }
        public string? Notes { get; set; }
    }
}
