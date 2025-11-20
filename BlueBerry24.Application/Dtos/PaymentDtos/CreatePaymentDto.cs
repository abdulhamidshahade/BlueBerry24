using BlueBerry24.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace BlueBerry24.Application.Dtos.PaymentDtos
{
    public class CreatePaymentDto
    {
        public int? OrderId { get; set; }


        public PaymentMethod Method { get; set; }

      
        public string Provider { get; set; } = string.Empty;

       
        
        public decimal Amount { get; set; }

        public string Currency { get; set; } = "USD";

        public string? ProviderPaymentMethodId { get; set; }

        // Payment Details
        public string? CardLast4 { get; set; }
        public string? CardBrand { get; set; }

        [EmailAddress]
        public string? PayerEmail { get; set; } = string.Empty;

        public string? PayerName { get; set; } = string.Empty;
  
        public string? BillingAddress1 { get; set; } = string.Empty;
        public string? BillingAddress2 { get; set; }

        public string BillingCity { get; set; } = string.Empty;

       
        public string BillingState { get; set; } = string.Empty;

    
        public string BillingPostalCode { get; set; } = string.Empty;

  
        public string BillingCountry { get; set; } = string.Empty;

        public string? Metadata { get; set; }
        public string? Notes { get; set; }
    }
}
