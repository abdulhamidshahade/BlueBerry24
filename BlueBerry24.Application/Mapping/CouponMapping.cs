using AutoMapper;
using BlueBerry24.Application.Dtos.CouponDtos;
using BlueBerry24.Domain.Entities.Coupon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Mapping
{
    class CouponMapping : Profile
    {
        public CouponMapping()
        {
            CreateMap<CouponDto, CreateCouponDto>().ReverseMap();
            CreateMap<CouponDto, UpdateCouponDto>().ReverseMap();
            CreateMap<CouponDto, DeleteCouponDto>().ReverseMap();
            CreateMap<CouponDto, Coupon>().ReverseMap();
        }
    }
}
