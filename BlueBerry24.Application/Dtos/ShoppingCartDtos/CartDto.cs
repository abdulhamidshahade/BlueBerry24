namespace BlueBerry24.Application.Dtos.ShoppingCartDtos
{
    public class CartDto
    {
        public CartHeaderDto ShoppingCartHeaderDto { get; set; }
        public List<CartItemDto> CartItems { get; set; }
    }
}
