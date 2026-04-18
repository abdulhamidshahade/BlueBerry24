using AutoMapper;
using Berryfy.Application.Dtos.CouponDtos;
using Berryfy.Domain.Entities.CouponEntities;
namespace Berryfy.Application.Mapping
{
    public class CouponMapping : Profile
    {
        public CouponMapping()
        {
            CreateMap<CouponDto, CreateCouponDto>().ReverseMap();
            CreateMap<CouponDto, UpdateCouponDto>().ReverseMap();

            CreateMap<Coupon, CreateCouponDto>().ReverseMap();
            CreateMap<Coupon, UpdateCouponDto>().ReverseMap();

            CreateMap<Coupon, CouponDto>()
                .ForMember(d => d.DiscountAmount, o => o.MapFrom(s => s.DiscountAmount ?? 0))
                .ForMember(d => d.Value, o => o.MapFrom(s => s.Value ?? 0))
                .ForMember(d => d.MinimumOrderAmount, o => o.MapFrom(s => s.MinimumOrderAmount ?? 0));

            CreateMap<UserCoupon, UserCouponDto>().ReverseMap();
        }
    }
}
