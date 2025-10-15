namespace BlueBerry24.Application.Dtos.InventoryDtos
{
    public class AdjustStockRequest
    {
        public int ProductId { get; set; }
        public int NewQuantity { get; set; }
        public string Notes { get; set; } = string.Empty;
        public int? PerformedByUserId { get; set; }
    }
}
