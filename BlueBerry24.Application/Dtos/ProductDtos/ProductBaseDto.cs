using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Dtos.ProductDtos
{
    public abstract class ProductBaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string ImageUrl { get; set; }

        public int ReservedStock { get; set; }
        public int LowStockThreshold { get; set; } = 5;

        public bool IsActive { get; set; } = true;

        public string SKU { get; set; } = string.Empty;
    }
}
