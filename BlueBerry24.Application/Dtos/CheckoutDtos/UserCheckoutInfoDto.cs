namespace BlueBerry24.Application.Dtos.CheckoutDtos
{
    public class UserCheckoutInfoDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? SessionId { get; set; }
        
        // Checkout Information
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Address { get; set; } = string.Empty;
        public string? Address2 { get; set; }
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string Country { get; set; } = "US";
        
        // Payment Billing Information
        public string? PayerName { get; set; }
        public string? PayerEmail { get; set; }
        public string? BillingAddress1 { get; set; }
        public string? BillingAddress2 { get; set; }
        public string? BillingCity { get; set; }
        public string? BillingState { get; set; }
        public string? BillingPostalCode { get; set; }
        public string? BillingCountry { get; set; }
        
        public DateTime? LastUsedAt { get; set; }
    }

    public class SaveCheckoutInfoDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Address { get; set; } = string.Empty;
        public string? Address2 { get; set; }
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string Country { get; set; } = "US";
    }

    public class SavePaymentBillingDto
    {
        public string PayerName { get; set; } = string.Empty;
        public string PayerEmail { get; set; } = string.Empty;
        public string BillingAddress1 { get; set; } = string.Empty;
        public string? BillingAddress2 { get; set; }
        public string BillingCity { get; set; } = string.Empty;
        public string BillingState { get; set; } = string.Empty;
        public string BillingPostalCode { get; set; } = string.Empty;
        public string BillingCountry { get; set; } = "US";
    }
}
