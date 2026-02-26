namespace BlueBerry24.Application.Services.Interfaces.OrchestrationServiceInterfaces
{
    public interface IRefundOrchestrationService
    {
        Task<RefundResult> ProcessRefundAsync(int orderId, string reason, decimal? refundAmount = null, int? performedByUserId = null);
    }

    public class RefundResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public bool PaymentRefunded { get; set; }
        public bool InventoryRestored { get; set; }
        public bool CouponsReverted { get; set; }
        public decimal RefundedAmount { get; set; }
        public List<string> Warnings { get; set; } = new();
    }
}
