using BlueBerry24.Services.CouponAPI.Data;
using BlueBerry24.Services.CouponAPI.Services.Interfaces;

namespace BlueBerry24.Services.CouponAPI.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
