using Berryfy.Application.Dtos.CheckoutDtos;

namespace Berryfy.Application.Services.Interfaces.CheckoutServiceInterfaces
{
    public interface IUserCheckoutInfoService
    {
        Task<UserCheckoutInfoDto?> GetCheckoutInfoAsync(int userId);
        Task<UserCheckoutInfoDto> SaveCheckoutInfoAsync(int userId, SaveCheckoutInfoDto dto);
        Task<UserCheckoutInfoDto> SavePaymentBillingInfoAsync(int userId, SavePaymentBillingDto dto);
        Task<bool> DeleteCheckoutInfoAsync(int userId);
    }
}
