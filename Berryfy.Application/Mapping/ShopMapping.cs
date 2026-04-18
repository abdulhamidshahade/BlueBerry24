using AutoMapper;
using Berryfy.Application.Dtos.ShopDtos;
using Berryfy.Domain.Entities.ShopEntities;

namespace Berryfy.Application.Mapping
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
