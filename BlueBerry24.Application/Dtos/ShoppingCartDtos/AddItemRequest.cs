using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Dtos.ShoppingCartDtos
{
    public class AddItemRequest
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int? UserId { get; set; }
        public string? SessionId { get; set; }
        public int Quantity { get; set; }
    }
}
