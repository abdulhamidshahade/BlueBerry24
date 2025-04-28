namespace BlueBerry24.Domain.Entities.ShoppingCart
{
    public class CartHeader
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal Discount { get; set; }
        public string CouponCode { get; set; } = string.Empty;
        public decimal CartTotal { get; set; }

        public bool IsActive { get; set; }

        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
