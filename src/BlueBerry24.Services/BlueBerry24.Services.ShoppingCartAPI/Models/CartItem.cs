using BlueBerry24.Services.ShoppingCartAPI.Models.DTOs.DTOs;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlueBerry24.Services.ShoppingCartAPI.Models
{
    public class CartItem
    {
        public string Id { get; set; }
        public string CartHeaderId { get; set; }
        public CartHeader CartHeader { get; set; }

        public string ProductId { get; set; }

        [NotMapped]
        public ProductDto Product { get; set; }

        public int Count { get; set; }

    }
}
