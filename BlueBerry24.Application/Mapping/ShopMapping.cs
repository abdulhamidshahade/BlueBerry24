using AutoMapper;
using BlueBerry24.Application.Dtos.ShopDtos;
using BlueBerry24.Domain.Entities.ShopEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Mapping
{
    class ShopMapping : Profile
    {
        public ShopMapping()
        {
            CreateMap<ShopDto, CreateShopDto>().ReverseMap();
            CreateMap<ShopDto, UpdateShopDto>().ReverseMap();
            CreateMap<ShopDto, DeleteShopDto>().ReverseMap();
            CreateMap<ShopDto, Shop>().ReverseMap();
        }
    }
}
