﻿using BlueBerry24.Services.ProductAPI.Models;

namespace BlueBerry24.Services.ProductAPI.Services.Interfaces
{
    public interface IProductCategoryService
    {
        Task<bool> AddProductCategoryAsync(Product Product, List<int> categories);
        Task<bool> UpdateProductCategoryAsync(Product product, List<int> categories);
    }
}
