using BlueBerry24.Application.Dtos.ProductDtos;

namespace BlueBerry24.Application.Dtos.WishlistDtos
{
    public class WishlistItemDto
    {
        public int Id { get; set; }
        public int WishlistId { get; set; }
        public int ProductId { get; set; }
        public string? Notes { get; set; }
        public int Priority { get; set; }
        public DateTime AddedDate { get; set; }
        public ProductDto Product { get; set; }
    }
}
