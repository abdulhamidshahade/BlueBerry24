using BlueBerry24.Services.NotificationAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BlueBerry24.Services.NotificationAPI.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Notification> Notifications { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.Metadata)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, (JsonSerializerOptions)null))
                .HasColumnType("nvarchar(max)");
            });

        }
    }
}
