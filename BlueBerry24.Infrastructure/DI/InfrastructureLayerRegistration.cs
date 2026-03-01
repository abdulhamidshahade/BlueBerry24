using BlueBerry24.Domain.Repositories;
using BlueBerry24.Domain.Repositories.CheckoutInterfaces;
using BlueBerry24.Domain.Repositories.CouponInterfaces;
using BlueBerry24.Domain.Repositories.InventoryInterfaces;
using BlueBerry24.Domain.Repositories.OrderInterfaces;
using BlueBerry24.Domain.Repositories.PaymentInterfaces;
using BlueBerry24.Domain.Repositories.ProductInterfaces;
using BlueBerry24.Domain.Repositories.ShopInterfaces;
using BlueBerry24.Domain.Repositories.ShoppingCartInterfaces;
using BlueBerry24.Domain.Repositories.WishlistInterfaces;
using BlueBerry24.Infrastructure.Data;
using BlueBerry24.Infrastructure.Repositories.CheckoutConcretes;
using BlueBerry24.Infrastructure.Repositories.CouponConcretes;
using BlueBerry24.Infrastructure.Repositories.InventoryConcretes;
using BlueBerry24.Infrastructure.Repositories.OrderConcretes;
using BlueBerry24.Infrastructure.Repositories.PaymentConcretes;
using BlueBerry24.Infrastructure.Repositories.ProductConcretes;
using BlueBerry24.Infrastructure.Repositories.ShopConcretes;
using BlueBerry24.Infrastructure.Repositories.ShoppingCartConcretes;
using BlueBerry24.Infrastructure.Repositories.WishlistConcretes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// TODO: Use here Microsoft.Extensions.DependencyInjection namespace

namespace BlueBerry24.Infrastructure.DI
{
    public static class InfrastructureLayerRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection serviceDescriptors)
        {

            serviceDescriptors.AddDbContext<ApplicationDbContext>((provider, options) =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("MSSQLServer");

                options.UseSqlServer(connectionString,
                    sqloptions => {
                        sqloptions.MigrationsAssembly("BlueBerry24.Infrastructure");

                        sqloptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                        sqloptions.CommandTimeout(120);
                    });

                options.EnableSensitiveDataLogging(false);

                options.EnableServiceProviderCaching();
                options.EnableDetailedErrors(false);

                options.ConfigureWarnings(warnings =>
                    warnings.Log(RelationalEventId.MultipleCollectionIncludeWarning));
            });

            serviceDescriptors.AddScoped<ICouponRepository, CouponRepository>();
            serviceDescriptors.AddScoped<IUserCouponRepository, UserCouponRepository>();

            serviceDescriptors.AddScoped<ICategoryRepository, CategoryRepository>();
            serviceDescriptors.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            serviceDescriptors.AddScoped<IProductRepository, ProductRepository>();

            serviceDescriptors.AddScoped<ICartRepository, CartRepository>();
            
            serviceDescriptors.AddScoped<IUserCheckoutInfoRepository, UserCheckoutInfoRepository>();

            serviceDescriptors.AddScoped<IInventoryRepository, InventoryRepository>();
            serviceDescriptors.AddScoped<IShopRepository, ShopRepository>();

            serviceDescriptors.AddScoped<IUnitOfWork, UnitOfWork>();

            serviceDescriptors.AddScoped<IOrderRepository, OrderRepository>();

            serviceDescriptors.AddScoped<IWishlistRepository, WishlistRepository>();

            serviceDescriptors.Add(new ServiceDescriptor
            (
                typeof(IPaymentRepository),
                typeof(PaymentRepository),
                ServiceLifetime.Scoped
                ));

            return serviceDescriptors;
        }
    }
}
