namespace BlueBerry24.Domain.Entities.Stock
{
    public class Stock
    {
        public int Id { get; set; }

        public int ShopId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
