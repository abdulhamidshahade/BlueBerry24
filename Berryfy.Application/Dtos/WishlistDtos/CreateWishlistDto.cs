namespace Berryfy.Application.Dtos.WishlistDtos
{
    public class CreateWishlistDto
    {
        public string Name { get; set; } = "My Wishlist";
        public bool IsPublic { get; set; } = false;
    }
}
