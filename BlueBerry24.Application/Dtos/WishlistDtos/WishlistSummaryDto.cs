namespace BlueBerry24.Application.Dtos.WishlistDtos
{
    public class WishlistSummaryDto
    {
        public int TotalWishlists { get; set; }
        public int TotalItems { get; set; }
        public decimal TotalValue { get; set; }
        public List<WishlistDto> RecentWishlists { get; set; } = new List<WishlistDto>();
    }
}
