namespace BlueBerry24.Application.Dtos.InventoryDtos
{
    public class ReserveStockRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int ReferenceId { get; set; }
        public string ReferenceType { get; set; } = string.Empty;
    }
}
