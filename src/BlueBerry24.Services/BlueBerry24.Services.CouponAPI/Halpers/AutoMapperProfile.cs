using AutoMapper;
using BlueBerry24.Services.CouponAPI.Models.DTOs;
using BlueBerry24.Services.CouponAPI.Models;

namespace BlueBerry24.Services.CouponAPI.Halpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Coupon, CouponDto>().ReverseMap();
            CreateMap<Coupon, CreateCouponDto>().ReverseMap();
        }
    }
}
