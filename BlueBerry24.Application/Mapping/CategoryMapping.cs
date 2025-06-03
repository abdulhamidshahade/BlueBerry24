using AutoMapper;
using BlueBerry24.Application.Dtos.CategoryDtos;
using BlueBerry24.Domain.Entities.ProductEntities;

namespace BlueBerry24.Application.Mapping
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
