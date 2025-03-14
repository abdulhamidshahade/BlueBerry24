using BlueBerry24.Services.ShoppingCartAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BlueBerry24.Services.ShoppingCartAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<CartHeader> CartHeaders { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.CartHeader)
                .WithMany(ci => ci.CartItems)
                .HasForeignKey(fk => fk.CartHeaderId);
        }
    }
}
