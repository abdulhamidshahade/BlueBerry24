using AutoMapper;
using Berryfy.Application.Dtos.CategoryDtos;
using Berryfy.Domain.Entities.ProductEntities;

namespace Berryfy.Application.Mapping
{
    public class CategoryMapping : Profile
    {
        public CategoryMapping()
        {
            CreateMap<CategoryDto, CreateCategoryDto>().ReverseMap();
            CreateMap<CategoryDto, UpdateCategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();


            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.ProductCategories.Select(p => p.Product)));

        }
    }
}
