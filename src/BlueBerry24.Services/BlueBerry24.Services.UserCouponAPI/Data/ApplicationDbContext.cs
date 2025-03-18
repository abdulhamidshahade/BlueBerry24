using BlueBerry24.Services.UserCouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BlueBerry24.Services.UserCouponAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UserCoupon> Users_Coupons { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
