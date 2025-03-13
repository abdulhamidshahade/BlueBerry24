using BlueBerry24.Services.ShopAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BlueBerry24.Services.ShopAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Shop> Shops { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
