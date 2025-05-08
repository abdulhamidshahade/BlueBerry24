using System.ComponentModel.DataAnnotations;


namespace BlueBerry24.Domain.Entities.ShoppingCart
{
    public class CartItem
    {
        public int Id { get; set; }
        public int ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public int ProductId { get; set; }

        //[NotMapped]
        //public ProductDto Product { get; set; }
        public int Count { get; set; }


        //[Timestamp]
        //public byte[] RowVersion { get; set; }

    }
}
