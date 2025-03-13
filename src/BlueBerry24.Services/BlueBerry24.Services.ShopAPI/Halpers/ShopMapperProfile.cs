using AutoMapper;
using BlueBerry24.Services.ShopAPI.Models;
using BlueBerry24.Services.ShopAPI.Models.DTOs.ShopDtos;

namespace BlueBerry24.Services.ShopAPI.Halpers
{
    public class ShopMapperProfile : Profile
    {
        public ShopMapperProfile()
        {
            CreateMap<Shop, ShopDto>().ReverseMap();
            CreateMap<CreateShopDto, Shop>().ReverseMap();
            CreateMap<UpdateShopDto, Shop>().ReverseMap();
            CreateMap<DeleteShopDto, Shop>().ReverseMap();
        }
    }
}
