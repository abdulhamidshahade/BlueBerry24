using BlueBerry24.Services.StockAPI.Data;
using BlueBerry24.Services.StockAPI.Services.Interfaces;

namespace BlueBerry24.Services.StockAPI.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
