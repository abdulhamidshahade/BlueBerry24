

using Berryfy.Application.Dtos.ProductDtos;

namespace Berryfy.Application.Dtos.ShoppingCartDtos
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int ShoppingCartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public ProductDto Product { get; set; }
    }
}
