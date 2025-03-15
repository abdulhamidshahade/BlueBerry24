using System.ComponentModel.DataAnnotations.Schema;

namespace BlueBerry24.Services.ShoppingCartAPI.Models.DTOs
{
    public class CartItemDto
    {
        public string Id { get; set; }
        public string CartHeaderId { get; set; }
        public string ProductId { get; set; }
        public int Count { get; set; }
    }
}
