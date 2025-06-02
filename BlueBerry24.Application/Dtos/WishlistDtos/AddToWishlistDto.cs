namespace BlueBerry24.Application.Dtos.WishlistDtos
{
    public class AddToWishlistDto
    {
        public int ProductId { get; set; }
        public int? WishlistId { get; set; } // If null, add to default wishlist
        public string? Notes { get; set; }
        public int Priority { get; set; } = 1;
    }
}
