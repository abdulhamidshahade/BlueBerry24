namespace BlueBerry24.Domain.Entities.Stock
{
    public class Stock
    {
        public string Id { get; set; }

        public string ShopId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
