﻿namespace BlueBerry24.Services.ProductAPI.Models
{
    public class Product : ProductBase
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
