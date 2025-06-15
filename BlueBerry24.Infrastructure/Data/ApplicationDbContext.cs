using BlueBerry24.Domain.Entities.AuthEntities;
using BlueBerry24.Domain.Entities.Base;
using BlueBerry24.Domain.Entities.CouponEntities;
using BlueBerry24.Domain.Entities.InventoryEntities;
using BlueBerry24.Domain.Entities.OrderEntities;
using BlueBerry24.Domain.Entities.PaymentEntities;
using BlueBerry24.Domain.Entities.ProductEntities;
using BlueBerry24.Domain.Entities.ShopEntities;
using BlueBerry24.Domain.Entities.ShoppingCartEntities;
using BlueBerry24.Domain.Entities.WishlistEntities;
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
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Cart> ShoppingCarts { get; set; }
        public DbSet<CartCoupon> CartCoupons { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<InventoryLog> InventoryLogs { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .HasMany(c => c.Cart)
                .WithOne(u => u.User)
                .HasForeignKey(fk => fk.UserId);

            builder.Entity<ApplicationUser>()
                .HasMany(o => o.Orders)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId);

            builder.Entity<UserCoupon>()
                .HasOne(u => u.User)
                .WithMany(uc => uc.UserCoupons)
                .HasForeignKey(fk => fk.UserId);

            builder.Entity<UserCoupon>()
                .HasOne(o => o.Coupon)
                .WithMany(uc => uc.UserCoupons)
                .HasForeignKey(fk => fk.CouponId);

            builder.Entity<CartCoupon>()
                .HasOne(c => c.Cart)
                .WithMany(cc => cc.CartCoupons)
                .HasForeignKey(cc => cc.CartId);

            builder.Entity<CartCoupon>()
                .HasOne(c => c.Coupon)
                .WithMany(cc => cc.CartCoupons)
                .HasForeignKey(fk => fk.CouponId);

            builder.Entity<InventoryLog>()
                .HasOne(p => p.Product)
                .WithMany(ig => ig.InventoryLogs)
                .HasForeignKey(p => p.ProductId);

            builder.Entity<ProductCategory>()
                .HasOne(p => p.Product)
                .WithMany(c => c.ProductCategories)
                .HasForeignKey(fk => fk.ProductId);

            builder.Entity<ProductCategory>()
                .HasOne(c => c.Category)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(fk => fk.CategoryId);

            builder.Entity<Order>()
                .HasMany(oi => oi.OrderItems)
                .WithOne(o => o.Order)
                .HasForeignKey(fk => fk.OrderId);

            builder.Entity<Order>()
                .HasOne(c => c.Cart)
                .WithOne(o => o.Order)
                .HasForeignKey<Order>(fk => fk.CartId);

            builder.Entity<OrderItem>()
                .HasOne(p => p.Product)
                .WithMany(oi => oi.OrderItems)
                .HasForeignKey(fk => fk.ProductId);

            builder.Entity<CartItem>()
                .HasOne(ci => ci.ShoppingCart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.ShoppingCartId);

            builder.Entity<Wishlist>()
                .HasMany(wli => wli.WishlistItems)
                .WithOne(wl => wl.Wishlist)
                .HasForeignKey(fk => fk.WishlistId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Wishlist>()
                .HasOne(u => u.User)
                .WithMany(w => w.Wishlists)
                .HasForeignKey(fk => fk.UserId);

            builder.Entity<Payment>()
                .HasOne(u => u.User)
                .WithMany(p => p.Payments)
                .HasForeignKey(u => u.UserId);

            builder.Entity<Payment>()
                .HasOne(o => o.Order)
                .WithMany(p => p.Payments)
                .HasForeignKey(o => o.OrderId);

        }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<IAuditableEntity>().
                Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                var entity = entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }

                entity.UpdatedAt = DateTime.UtcNow;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
