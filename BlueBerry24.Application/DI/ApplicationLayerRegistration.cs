using BlueBerry24.Application.Services.Concretes.AuthServiceConcretes;
using BlueBerry24.Application.Services.Concretes.CouponServiceConcretes;
using BlueBerry24.Application.Services.Concretes.EmailServiceConcretes;
using BlueBerry24.Application.Services.Concretes.InventoryServiceConcretes;
using BlueBerry24.Application.Services.Concretes.OrderServiceConcretes;
using BlueBerry24.Application.Services.Concretes.PaymentConcretes;
using BlueBerry24.Application.Services.Concretes.ProductServiceConcretes;
using BlueBerry24.Application.Services.Concretes.ShoppingCartServiceConcretes;
using BlueBerry24.Application.Services.Concretes.ShopServiceConcretes;
using BlueBerry24.Application.Services.Concretes.WishlistServiceConcretes;
using BlueBerry24.Application.Services.Interfaces.AuthServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.CouponServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.EmailServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.InventoryServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.OrderServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.PaymentServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.ProductServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.ShoppingCartServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.ShopServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.WishlistServiceInterfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

// TODO: Use here Microsoft.Extensions.DependencyInjection namespace

namespace BlueBerry24.Application.DI
{
    public static class ApplicationLayerRegistration
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection serviceDescriptors, 
            IHostBuilder hostBuilder,
            IConfiguration configuration)
        {
            serviceDescriptors.AddScoped<IAuthService, AuthService>();
            serviceDescriptors.AddScoped<ITokenService, TokenService>();
            serviceDescriptors.AddScoped<IUserService, UserService>();

            serviceDescriptors.AddScoped<ICouponService, CouponService>();
            serviceDescriptors.AddScoped<IUserCouponService, UserCouponService>();

            serviceDescriptors.AddScoped<ICategoryService, CategoryService>();
            serviceDescriptors.AddScoped<IProductCategoryService, ProductCategoryService>();
            serviceDescriptors.AddScoped<IProductService, ProductService>();

            serviceDescriptors.AddScoped<ICartService, CartService>();
            serviceDescriptors.AddScoped<IShopService, ShopService>();

            serviceDescriptors.AddScoped<IInventoryService, InventoryService>();

            serviceDescriptors.AddScoped<IOrderService, OrderService>();

            serviceDescriptors.AddScoped<IRoleManagementService, RoleManagementService>();

            serviceDescriptors.AddScoped<IWishlistService, WishlistService>();

            serviceDescriptors.AddScoped<IPaymentService, PaymentService>();

            serviceDescriptors.AddScoped<IMailService, GmailService>();


            hostBuilder.UseSerilog((context, loggerConfig) =>
            {
                loggerConfig.ReadFrom.Configuration(configuration);
            });
          

            return serviceDescriptors;
        }
    }
}
