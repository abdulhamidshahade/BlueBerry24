using BlueBerry24.Application.Dtos.ProductDtos;
using BlueBerry24.Domain.Entities.ProductEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Services.Interfaces.ProductServiceInterfaces
{
    public interface IProductCategoryService
    {
        Task<bool> AddProductCategoryAsync(ProductDto Product, List<int> categories);
        Task<bool> UpdateProductCategoryAsync(ProductDto product, List<int> categories);
    }
}
