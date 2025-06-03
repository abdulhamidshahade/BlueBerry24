using AutoMapper;
using BlueBerry24.Application.Dtos.ShopDtos;
using BlueBerry24.Domain.Entities.ShopEntities;

namespace BlueBerry24.Application.Mapping
{
    public class ShopMapping : Profile
    {
        public ShopMapping()
        {
            CreateMap<ShopDto, UpdateShopDto>().ReverseMap();
            CreateMap<ShopDto, Shop>().ReverseMap();
            CreateMap<Shop, UpdateShopDto>().ReverseMap();
        }
    }
}
