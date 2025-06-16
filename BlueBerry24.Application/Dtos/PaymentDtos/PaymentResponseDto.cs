using BlueBerry24.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Dtos.PaymentDtos
{
    public class PaymentResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public PaymentDto? Payment { get; set; }
        public string? TransactionId { get; set; }
        public PaymentStatus Status { get; set; }
        public string? RedirectUrl { get; set; }
        public Dictionary<string, object>? Metadata { get; set; }
    }
}
