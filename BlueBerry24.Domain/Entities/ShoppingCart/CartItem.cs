using System.ComponentModel.DataAnnotations;


namespace BlueBerry24.Domain.Entities.ShoppingCart
{
    public class CartItem
    {
        public int Id { get; set; }
        public string CartHeaderId { get; set; }
        public CartHeader CartHeader { get; set; }

        public string ProductId { get; set; }
        public string ShopId { get; set; }

        //[NotMapped]
        //public ProductDto Product { get; set; }

        public int Count { get; set; }


        [Timestamp]
        public byte[] RowVersion { get; set; }

    }
}
