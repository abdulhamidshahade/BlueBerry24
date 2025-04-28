using BlueBerry24.Domain.Entities.Auth;
using BlueBerry24.Domain.Entities.Coupon;
using BlueBerry24.Domain.Entities.Product;
using BlueBerry24.Domain.Entities.Shop;
using BlueBerry24.Domain.Entities.ShoppingCart;
using BlueBerry24.Domain.Entities.Stock;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlueBerry24.Infrastructure.Data
{
    class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<CartHeader> CartHeaders { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ProductCategory>()
                .HasOne(p => p.Product)
                .WithMany(c => c.ProductCategories)
                .HasForeignKey(fk => fk.ProductId);

            builder.Entity<ProductCategory>()
                .HasOne(c => c.Category)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(fk => fk.CategoryId);
        }
    }
}
