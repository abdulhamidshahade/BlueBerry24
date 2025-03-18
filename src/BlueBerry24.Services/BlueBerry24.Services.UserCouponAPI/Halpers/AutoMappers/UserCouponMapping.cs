using AutoMapper;
using BlueBerry24.Services.UserCouponAPI.Models;
using BlueBerry24.Services.UserCouponAPI.Models.DTOs;

namespace BlueBerry24.Services.UserCouponAPI.Halpers.AutoMappers
{
    public class UserCouponMapping : Profile
    {
        public UserCouponMapping()
        {
            CreateMap<UserCoupon, UserCouponDto>().ReverseMap();
        }
    }
}
