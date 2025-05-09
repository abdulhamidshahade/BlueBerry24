﻿using BlueBerry24.Domain.Entities.Base;
using BlueBerry24.Domain.Entities.ShopEntities;

namespace BlueBerry24.Domain.Entities.ProductEntities
{
    public class Product : ProductBase, IAuditableEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public int ShopId { get; set; }
        public int StockId { get; set; }
        public Shop Shop { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
    }
}
