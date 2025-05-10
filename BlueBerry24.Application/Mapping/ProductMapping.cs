using AutoMapper;
using BlueBerry24.Application.Dtos.ProductDtos;
using BlueBerry24.Domain.Entities.ProductEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Mapping
{
    class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<ProductDto, CreateProductDto>().ReverseMap();
            CreateMap<ProductDto, UpdateProductDto>().ReverseMap();
            CreateMap<ProductDto, DeleteProductDto>().ReverseMap();
            CreateMap<ProductDto, Product>().ReverseMap();
        }
    }
}
