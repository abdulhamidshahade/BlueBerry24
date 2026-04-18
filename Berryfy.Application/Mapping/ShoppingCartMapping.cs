using AutoMapper;
using Berryfy.Application.Dtos.ShoppingCartDtos;
using Berryfy.Domain.Entities.ShoppingCartEntities;

namespace Berryfy.Application.Mapping
{
    public class ShoppingCartMapping : Profile
    {
        public ShoppingCartMapping()
        {

            CreateMap<CartItem, CartItemDto>().ReverseMap();
            CreateMap<CartCoupon, CartCouponDto>().ReverseMap();


            CreateMap<Cart, CartDto>()
            .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.SubTotal))
            .ForMember(dest => dest.DiscountTotal, opt => opt.MapFrom(src => src.DiscountTotal))
            .ForMember(dest => dest.TaxAmount, opt => opt.MapFrom(src => src.TaxAmount))
            .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total))
            .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems))
            .ForMember(dest => dest.CartCoupons, opt => opt.MapFrom(src => src.CartCoupons));

            CreateMap<CartDto, Cart>()
                .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems))
                .ForMember(dest => dest.CartCoupons, opt => opt.MapFrom(src => src.CartCoupons))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Version, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
        }
    }
}
