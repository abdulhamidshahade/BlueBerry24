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
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<ProductDto, CreateProductDto>().ReverseMap();
            CreateMap<ProductDto, UpdateProductDto>().ReverseMap();
            CreateMap<ProductDto, DeleteProductDto>().ReverseMap();
            


            CreateMap<Product, CreateProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap();


            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ProductCategoryNames,
                            opt => opt.MapFrom(src => src.ProductCategories.Select(c => c.Category.Name)));

            CreateMap<ProductDto, Product>();
        }
    }
}
