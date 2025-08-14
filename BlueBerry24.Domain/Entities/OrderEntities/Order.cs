using BlueBerry24.Domain.Constants;
using BlueBerry24.Domain.Entities.AuthEntities;
using BlueBerry24.Domain.Entities.Base;
using BlueBerry24.Domain.Entities.PaymentEntities;
using BlueBerry24.Domain.Entities.ShoppingCartEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Domain.Entities.OrderEntities
{
    public class Order : IAuditableEntity
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public int CartId { get; set; }
        public string? sessionId { get; set; }
        public bool isPaid { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal ShippingAmount { get; set; }
        public decimal Total { get; set; }

        public decimal DiscountTotal { get; set; }

        public string CustomerEmail { get; set; }
        public string? CustomerPhone { get; set; }


        public string? ReferenceNumber { get; set; }

        public DateTime? CompletedAt { get; set; }
        public DateTime? CancalledAt { get; set; }

        public string? ShippingName { get; set; }
        public string? ShippingAddress1 { get; set; }
        public string? ShippingAddress2 { get; set; }

        public string? ShippingCity { get; set; }

        public string? ShippingState { get; set; }

        public string? ShippingPostalCode { get; set; }

        public string? ShippingCountry { get; set; }


        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }



        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();


        public ApplicationUser User { get; set; }
        public Cart Cart { get; set; }

        public List<Payment> Payments { get; set; }
    }
}
