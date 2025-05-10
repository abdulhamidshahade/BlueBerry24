using BlueBerry24.Domain.Repositories;
using BlueBerry24.Domain.Repositories.CouponInterfaces;
using BlueBerry24.Domain.Repositories.ProductInterfaces;
using BlueBerry24.Domain.Repositories.ShopInterfaces;
using BlueBerry24.Domain.Repositories.ShoppingCartInterfaces;
using BlueBerry24.Domain.Repositories.ShoppingCartInterfaces.Cache;
using BlueBerry24.Domain.Repositories.StockInterfaces;
using BlueBerry24.Infrastructure.Data;
using BlueBerry24.Infrastructure.Repositories.CouponConcretes;
using BlueBerry24.Infrastructure.Repositories.ProductConcretes;
using BlueBerry24.Infrastructure.Repositories.ShopConcretes;
using BlueBerry24.Infrastructure.Repositories.ShoppingCartConcretes;
using BlueBerry24.Infrastructure.Repositories.ShoppingCartConcretes.Cache;
using BlueBerry24.Infrastructure.Repositories.StockConcretes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            serviceDescriptors.AddScoped<IShopRepository, ShopRepository>();

            serviceDescriptors.AddScoped<ICartHeaderCacheRepository, RedisCartHeaderCache>();
            serviceDescriptors.AddScoped<ICartItemCacheRepository, RedisCartItemCache>();

            serviceDescriptors.AddScoped<ICartHeaderRepository, CartHeaderRepository>();
            serviceDescriptors.AddScoped<ICartItemRepository, CartItemRepository>();
            serviceDescriptors.AddScoped<ICartRepository, CartRepository>();

            serviceDescriptors.AddScoped<IStockRepository, StockRepository>();

            serviceDescriptors.AddScoped<IUnitOfWork, UnitOfWork>();

            serviceDescriptors.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var redisConnectionString = configuration.GetConnectionString("Redis");

                if (string.IsNullOrEmpty(redisConnectionString))
                {
                    throw new InvalidOperationException("Redis connection string is missing in configuration.");
                }

                var options = ConfigurationOptions.Parse(redisConnectionString);
                return ConnectionMultiplexer.Connect(options);
            });



            return serviceDescriptors;
        }
    }
}
