using BlueBerry24.Services.NotificationAPI.Data.Context;
using BlueBerry24.Services.NotificationAPI.Services.Interfaces;

namespace BlueBerry24.Services.NotificationAPI.Services
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
    }
}
