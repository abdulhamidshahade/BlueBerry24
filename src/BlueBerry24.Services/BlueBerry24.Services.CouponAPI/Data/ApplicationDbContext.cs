using BlueBerry24.Services.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BlueBerry24.Services.CouponAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Coupon> Coupons { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
