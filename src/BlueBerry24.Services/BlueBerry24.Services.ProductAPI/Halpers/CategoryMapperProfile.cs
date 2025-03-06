using AutoMapper;
using BlueBerry24.Services.ProductAPI.Models;
using BlueBerry24.Services.ProductAPI.Models.DTOs.CategoryDtos;
using BlueBerry24.Services.ProductAPI.Models.DTOs.ProductDtos;

namespace BlueBerry24.Services.ProductAPI.Halpers
{
    public class CategoryMapperProfile : Profile
    {
        public CategoryMapperProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Category, DeleteCategoryDto>().ReverseMap();
        }
    }
}
