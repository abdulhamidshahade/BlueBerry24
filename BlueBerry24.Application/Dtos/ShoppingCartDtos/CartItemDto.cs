

namespace BlueBerry24.Application.Dtos.ShoppingCartDtos
{
    public class CartItemDto
    {
        public string Id { get; set; }
        public string CartHeaderId { get; set; }
        public string ProductId { get; set; }
        public string ShopId { get; set; }
        public int Count { get; set; }
        public decimal unitPrice { get; set; }
    }
}
