using AutoMapper;
using BlueBerry24.Application.Dtos.CategoryDtos;
using BlueBerry24.Domain.Entities.ProductEntities;

namespace BlueBerry24.Application.Mapping
{
    class CategoryMapping : Profile
    {
        public CategoryMapping()
        {
            CreateMap<CategoryDto, CreateCategoryDto>().ReverseMap();
            CreateMap<CategoryDto, UpdateCategoryDto>().ReverseMap();
            CreateMap<CategoryDto, DeleteCategoryDto>().ReverseMap();
            CreateMap<CategoryDto, Category>().ReverseMap();
        }
    }
}
