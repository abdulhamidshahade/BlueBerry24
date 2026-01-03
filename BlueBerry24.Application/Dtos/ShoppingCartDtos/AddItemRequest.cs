namespace BlueBerry24.Application.Dtos.ShoppingCartDtos
{
    public class AddItemRequest
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int? UserId { get; set; }
        public string? SessionId { get; set; }
        public int Quantity { get; set; }
    }
}
