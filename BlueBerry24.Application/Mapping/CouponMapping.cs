using AutoMapper;
using BlueBerry24.Application.Dtos.CouponDtos;
using BlueBerry24.Domain.Entities.CouponEntities;
namespace BlueBerry24.Application.Mapping
{
    public class CouponMapping : Profile
    {
        public CouponMapping()
        {
            CreateMap<CouponDto, CreateCouponDto>().ReverseMap();
            CreateMap<CouponDto, UpdateCouponDto>().ReverseMap();
            CreateMap<CouponDto, Coupon>().ReverseMap();

            CreateMap<Coupon, CreateCouponDto>().ReverseMap();
            CreateMap<Coupon, UpdateCouponDto>().ReverseMap();

            CreateMap<UserCoupon, UserCouponDto>().ReverseMap();
        }
    }
}
