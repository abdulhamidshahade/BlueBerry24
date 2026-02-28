using BlueBerry24.Domain.Entities.CheckoutEntities;
using BlueBerry24.Domain.Repositories.CheckoutInterfaces;
using BlueBerry24.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlueBerry24.Infrastructure.Repositories.CheckoutConcretes
{
    public class UserCheckoutInfoRepository : IUserCheckoutInfoRepository
    {
        private readonly ApplicationDbContext _context;

        public UserCheckoutInfoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserCheckoutInfo?> GetByUserIdAsync(int userId)
        {
            return await _context.UserCheckoutInfos
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<UserCheckoutInfo?> GetBySessionIdAsync(string sessionId)
        {
            return await _context.UserCheckoutInfos
                .FirstOrDefaultAsync(x => x.SessionId == sessionId);
        }

        public async Task<UserCheckoutInfo> CreateAsync(UserCheckoutInfo checkoutInfo)
        {
            checkoutInfo.CreatedAt = DateTime.UtcNow;
            checkoutInfo.UpdatedAt = DateTime.UtcNow;
            checkoutInfo.LastUsedAt = DateTime.UtcNow;
            
            _context.UserCheckoutInfos.Add(checkoutInfo);
            await _context.SaveChangesAsync();
            return checkoutInfo;
        }

        public async Task<bool> UpdateAsync(UserCheckoutInfo checkoutInfo)
        {
            checkoutInfo.UpdatedAt = DateTime.UtcNow;
            checkoutInfo.LastUsedAt = DateTime.UtcNow;
            
            _context.UserCheckoutInfos.Update(checkoutInfo);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var checkoutInfo = await _context.UserCheckoutInfos.FindAsync(id);
            if (checkoutInfo == null) return false;

            _context.UserCheckoutInfos.Remove(checkoutInfo);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateLastUsedAsync(int id)
        {
            var checkoutInfo = await _context.UserCheckoutInfos.FindAsync(id);
            if (checkoutInfo == null) return false;

            checkoutInfo.LastUsedAt = DateTime.UtcNow;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
