using Berryfy.Domain.Entities.CheckoutEntities;

namespace Berryfy.Domain.Repositories.CheckoutInterfaces
{
    public interface IUserCheckoutInfoRepository
    {
        Task<UserCheckoutInfo?> GetByUserIdAsync(int userId);
        Task<UserCheckoutInfo?> GetBySessionIdAsync(string sessionId);
        Task<UserCheckoutInfo> CreateAsync(UserCheckoutInfo checkoutInfo);
        Task<bool> UpdateAsync(UserCheckoutInfo checkoutInfo);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateLastUsedAsync(int id);
    }
}
