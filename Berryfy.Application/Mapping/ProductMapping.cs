using AutoMapper;
using Berryfy.Application.Dtos.ProductDtos;
using Berryfy.Domain.Entities.ProductEntities;
namespace Berryfy.Application.Mapping
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
                .ForMember(dest => dest.ProductCategories,
                            opt => opt.MapFrom(src => src.ProductCategories.Select(c => c.Category)));

            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.ProductCategories, opt => opt.Ignore());
            
        }
    }
}
