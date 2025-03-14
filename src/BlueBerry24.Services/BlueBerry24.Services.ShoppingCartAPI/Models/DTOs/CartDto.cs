namespace BlueBerry24.Services.ShoppingCartAPI.Models.DTOs
{
    public class CartDto
    {
        public CartHeaderDto ShoppingCartHeaderDto { get; set; }
        public IEnumerable<CartItemDto> CartItems { get; set; }
    }
}
