using BlueBerry24.Application.Dtos.OrderDtos;
using BlueBerry24.Domain.Entities.OrderEntities;

namespace BlueBerry24.Application.Services.Interfaces.OrchestrationServiceInterfaces
{
    public interface ICheckoutOrchestrationService
    {
        Task<CheckoutResult> ProcessCheckoutAsync(int cartId, CreateOrderDto orderDto, int? userId);
    }

    public class CheckoutResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public Order? Order { get; set; }
        public List<string> Warnings { get; set; } = new();
    }
}
