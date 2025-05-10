using BlueBerry24.Application.Services.Concretes.AuthServiceConcretes;
using BlueBerry24.Application.Services.Concretes.CouponServiceConcretes;
using BlueBerry24.Application.Services.Concretes.ProductServiceConcretes;
using BlueBerry24.Application.Services.Concretes.ShoppingCartServiceConcretes.Cache;
using BlueBerry24.Application.Services.Concretes.ShoppingCartServiceConcretes;
using BlueBerry24.Application.Services.Concretes.ShopServiceConcretes;
using BlueBerry24.Application.Services.Concretes.StockServiceConcretes;
using BlueBerry24.Application.Services.Interfaces.AuthServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.CouponServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.ProductServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.ShoppingCartServiceInterfaces.Cache;
using BlueBerry24.Application.Services.Interfaces.ShoppingCartServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.ShopServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.StockServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;


namespace BlueBerry24.Application.DI
{
    public static class ApplicationLayerRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddScoped<IAuthService, AuthService>();
            serviceDescriptors.AddScoped<IRoleService, RoleService>();
            serviceDescriptors.AddScoped<ITokenService, TokenService>();
            serviceDescriptors.AddScoped<IUserService, UserService>();

            serviceDescriptors.AddScoped<ICouponService, CouponService>();
            serviceDescriptors.AddScoped<IUserCouponService, UserCouponService>();

            serviceDescriptors.AddScoped<ICategoryService, CategoryService>();
            serviceDescriptors.AddScoped<IProductCategoryService, ProductCategoryService>();
            serviceDescriptors.AddScoped<IProductService, ProductService>();

            serviceDescriptors.AddScoped<ICartCacheService, CartCacheService>();
            serviceDescriptors.AddScoped<ICartHeaderCacheService, CartHeaderCacheService>();
            serviceDescriptors.AddScoped<ICartItemCacheService, CartItemCacheService>();

            serviceDescriptors.AddScoped<ICartHeaderService, CartHeaderService>();
            serviceDescriptors.AddScoped<ICartItemService, CartItemService>();
            serviceDescriptors.AddScoped<ICartService, CartService>();

            serviceDescriptors.AddScoped<IShopService, ShopService>();

            serviceDescriptors.AddScoped<IStockService, StockService>();

            return serviceDescriptors;
        }
    }
}
