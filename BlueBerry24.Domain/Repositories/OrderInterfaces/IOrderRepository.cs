using BlueBerry24.Domain.Constants;
using BlueBerry24.Domain.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Domain.Repositories.OrderInterfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<List<Order>> GetUserOrdersAsync(string sessionId, int page = 1, int pageSize = 10);
        Task<List<Order>> GetAllOrdersAsync(int page = 1, int pageSize = 50);
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus);
        Task<bool> MarkOrderAsPaidAsync(int orderId, int paymentTransactionId, string paymentProvider);
        Task<Order> CreateOrderAsync(Order order);
        Task<OrderItem> CreateOrderItemAsync(OrderItem item);
        Task<Order?> GetOrderByReferenceNumberAsync(string referenceNumber);
        Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status, int page = 1, int pageSize = 10);
    }
}
