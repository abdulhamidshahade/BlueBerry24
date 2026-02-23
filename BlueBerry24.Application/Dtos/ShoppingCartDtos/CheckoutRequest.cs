namespace BlueBerry24.Application.Dtos.ShoppingCartDtos
{
    public class CheckoutRequest
    {
        public int CartId { get; set; }
        public string CustomerEmail { get; set; } = string.Empty;
        public string? CustomerPhone { get; set; }
        public string ShippingName { get; set; } = string.Empty;
        public string ShippingAddressLine1 { get; set; } = string.Empty;
        public string? ShippingAddressLine2 { get; set; }
        public string ShippingCity { get; set; } = string.Empty;
        public string ShippingState { get; set; } = string.Empty;
        public string ShippingPostalCode { get; set; } = string.Empty;
        public string? ShippingCountry { get; set; }
        public string PaymentProvider { get; set; } = string.Empty;
        public int PaymentTransactionId { get; set; }
        public bool IsPaid { get; set; }
    }

}

