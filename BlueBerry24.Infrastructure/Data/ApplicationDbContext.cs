using BlueBerry24.Domain.Entities.AuthEntities;
using BlueBerry24.Domain.Entities.Base;
using BlueBerry24.Domain.Entities.CouponEntities;
using BlueBerry24.Domain.Entities.ProductEntities;
using BlueBerry24.Domain.Entities.ShopEntities;
using BlueBerry24.Domain.Entities.ShoppingCartEntities;
using BlueBerry24.Domain.Entities.StockEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlueBerry24.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<UserCoupon> UserCoupons { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<CartHeader> CartHeaders { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
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


            builder.Entity<UserCoupon>()
                .HasOne(u => u.User)
                .WithMany(uc => uc.UserCoupons)
                .HasForeignKey(fk => fk.UserId);

            builder.Entity<UserCoupon>()
                .HasOne(o => o.Coupon)
                .WithMany(uc => uc.UserCoupons)
                .HasForeignKey(fk => fk.CouponId);


            builder.Entity<Shop>()
                .HasMany(p => p.Products)
                .WithOne(s => s.Shop)
                .HasForeignKey(fk => fk.ShopId);

            builder.Entity<ShoppingCart>()
                .HasMany(ci => ci.CartItems)
                .WithOne(s => s.ShoppingCart)
                .HasForeignKey(fk => fk.ShoppingCartId);


            builder.Entity<ShoppingCart>()
                .HasOne(ch => ch.CartHeader)
                .WithOne(sc => sc.ShoppingCart)
                .HasForeignKey<CartHeader>(fk => fk.ShoppingCartId);

            builder.Entity<Product>()
                .HasOne(s => s.Stock)
                .WithOne(p => p.Product)
                .HasForeignKey<Stock>(s => s.ProductId);
        }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<IAuditableEntity>().
                Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach(var entry in entries)
            {
                var entity = entry.Entity;

                if(entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }

                entity.UpdatedAt = DateTime.UtcNow;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
