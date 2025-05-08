namespace BlueBerry24.Application.Dtos.ShoppingCartDtos
{
    public class CartDto
    {
        public int CartHeaderId { get; set; }
        public bool IsActive { get; set; }
        public int UserId { get; set; }
        public CartHeaderDto ShoppingCartHeaderDto { get; set; }
        public List<CartItemDto> CartItems { get; set; }
    }
}
