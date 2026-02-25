using BlueBerry24.Application.Dtos.OrderDtos;
using BlueBerry24.Domain.Constants;
using BlueBerry24.Domain.Entities.OrderEntities;
namespace BlueBerry24.Application.Services.Interfaces.OrderServiceInterfaces
{
    public interface IOrderService
    {
        Task<OrderDto?> GetOrderByIdAsync(int orderId);

        Task<List<OrderDto>> GetUserOrdersAsync(int userId, int page = 1, int pageSize = 10);
        Task<List<OrderDto>> GetAllOrdersAsync(int page = 1, int pageSize = 50);
        Task<Order?> CreateOrderFromCartAsync(int cartId, CreateOrderDto orderDto);
        Task<bool> UpdateOrderStatusAsync(Order order, OrderStatus newStatus);
        Task<bool> UpdateOrderPaymentStatusAsync(Order order, PaymentStatus paymentStatus);
        Task<bool> CancelOrderAsync(int orderId, string reason);
        Task<bool> RefundOrderAsync(int orderId, string reason);
        Task<bool> ProcessOrderAsync(int orderId);
        Task<OrderTotal> CalculateOrderTotalsAsync(int cartId);
        Task<string> GenerateUniqueReferenceNumberAsync();
        Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status, int page = 1, int pageSize = 10);
        Task<Order?> GetOrderByReferenceNumberAsync(string referenceNumber);
    }
}
