using BlueBerry24.Domain.Entities.WishlistEntities;
using BlueBerry24.Domain.Repositories;
using BlueBerry24.Domain.Repositories.CouponInterfaces;
using BlueBerry24.Domain.Repositories.InventoryInterfaces;
using BlueBerry24.Domain.Repositories.OrderInterfaces;
using BlueBerry24.Domain.Repositories.PaymentInterfaces;
using BlueBerry24.Domain.Repositories.ProductInterfaces;
using BlueBerry24.Domain.Repositories.ShopInterfaces;
using BlueBerry24.Domain.Repositories.ShoppingCartInterfaces;
using BlueBerry24.Domain.Repositories.WishlistInterfaces;
using BlueBerry24.Infrastructure.Data;
using BlueBerry24.Infrastructure.Repositories.CouponConcretes;
using BlueBerry24.Infrastructure.Repositories.InventoryConcretes;
using BlueBerry24.Infrastructure.Repositories.OrderConcretes;
using BlueBerry24.Infrastructure.Repositories.PaymentConcretes;
using BlueBerry24.Infrastructure.Repositories.ProductConcretes;
using BlueBerry24.Infrastructure.Repositories.ShopConcretes;
using BlueBerry24.Infrastructure.Repositories.ShoppingCartConcretes;
using BlueBerry24.Infrastructure.Repositories.WishlistConcretes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace BlueBerry24.Infrastructure.DI
{
    public static class InfrastructureLayerRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection serviceDescriptors)
        {

            serviceDescriptors.AddDbContext<ApplicationDbContext>((provider, options) =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("MSSQLServer");

                options.UseSqlServer(connectionString,
                    sqloptions => sqloptions.MigrationsAssembly("BlueBerry24.Infrastructure"));
            });

            serviceDescriptors.AddScoped<ICouponRepository, CouponRepository>();
            serviceDescriptors.AddScoped<IUserCouponRepository, UserCouponRepository>();

            serviceDescriptors.AddScoped<ICategoryRepository, CategoryRepository>();
            serviceDescriptors.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            serviceDescriptors.AddScoped<IProductRepository, ProductRepository>();

            serviceDescriptors.AddScoped<ICartRepository, CartRepository>();

            serviceDescriptors.AddScoped<IInventoryRepository, InventoryRepository>();
            serviceDescriptors.AddScoped<IShopRepository, ShopRepository>();

            serviceDescriptors.AddScoped<IUnitOfWork, UnitOfWork>();

            serviceDescriptors.AddScoped<IOrderRepository, OrderRepository>();

            serviceDescriptors.AddScoped<IWishlistRepository, WishlistRepository>();

            serviceDescriptors.AddScoped<IPaymentRepository, PaymentRepository>();

            return serviceDescriptors;
        }
    }
}
