using BlueBerry24.Services.ProductAPI.Models.DTOs.ProductDtos;
using BlueBerry24.Services.ProductAPI.Models;
using AutoMapper;

namespace BlueBerry24.Services.ProductAPI.Halpers
{
    public class ProductMapperProfile : Profile
    {
        public ProductMapperProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap();
            CreateMap<Product, CreateProductDto>().ReverseMap();
            CreateMap<Product, DeleteProductDto>().ReverseMap();
        }
    }
}
