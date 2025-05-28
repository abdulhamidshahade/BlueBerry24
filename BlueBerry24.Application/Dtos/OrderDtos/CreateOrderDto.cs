using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Dtos.OrderDtos
{
    public class CreateOrderDto
    {
        public int UserId { get; set; }
        public int CartId { get; set; }
        public string CustomerEmail { get; set; } = string.Empty;
        public string? CustomerPhone { get; set; }
        public string? ShippingName { get; set; }
        public string? ShippingAddressLine1 { get; set; }
        public string? ShippingAddressLine2 { get; set; }
        public string? ShippingCity { get; set; }
        public string? ShippingState { get; set; }
        public string? ShippingPostalCode { get; set; }
        public string? ShippingCountry { get; set; }
        public string? PaymentProvider { get; set; }
        public int PaymentTransactionId { get; set; }
        public bool IsPaid { get; set; }
    }
}
