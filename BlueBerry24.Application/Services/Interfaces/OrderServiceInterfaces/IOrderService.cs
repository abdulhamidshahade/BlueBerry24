using BlueBerry24.Application.Dtos.OrderDtos;
using BlueBerry24.Domain.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Services.Interfaces.OrderServiceInterfaces
{
    public interface IOrderService
    {
        Task<Order?> GetOrderByIdAsync(int orderId);

        Task<List<Order>> GetUserOrdersAsync(int userId, int page = 1, int pageSize = 10);
        Task<Order?> CreateOrderFromCartAsync(int cartId, CreateOrderDto orderDto);
        Task<bool> UpdateOrderStatusAsync(Order order, OrderStatus newStatus);
        Task<bool> MarkOrderAsPaidAsync(int orderId, int paymentTransactionId, string paymentProvider);
        Task<bool> CancelOrderAsync(int orderId, string reason);
        Task<bool> RefundOrderAsync(int orderId, string reason);
        Task<OrderTotal> CalculateOrderTotalsAsync(int cartId);
        Task<string> GenerateUniqueReferenceNumberAsync();
        Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status, int page = 1, int pageSize = 10);
        Task<Order?> GetOrderByReferenceNumberAsync(string referenceNumber);
    }
}
