using BlueBerry24.Application.Dtos.CheckoutDtos;

namespace BlueBerry24.Application.Services.Interfaces.CheckoutServiceInterfaces
{
    public interface IUserCheckoutInfoService
    {
        Task<UserCheckoutInfoDto?> GetCheckoutInfoAsync(int userId);
        Task<UserCheckoutInfoDto> SaveCheckoutInfoAsync(int userId, SaveCheckoutInfoDto dto);
        Task<UserCheckoutInfoDto> SavePaymentBillingInfoAsync(int userId, SavePaymentBillingDto dto);
        Task<bool> DeleteCheckoutInfoAsync(int userId);
    }
}
