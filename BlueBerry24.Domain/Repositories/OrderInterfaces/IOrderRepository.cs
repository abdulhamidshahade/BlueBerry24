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
        Task<Order?> GetOrderByIdAsync(int orderId, OrderStatus? orderStatus = OrderStatus.Pending);
        Task<List<Order>> GetUserOrdersAsync(int userId, int page = 1, int pageSize = 10);
        Task<List<Order>> GetAllOrdersAsync(int page = 1, int pageSize = 50);
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus);
        Task<Order> CreateOrderAsync(Order order);
        Task<OrderItem> CreateOrderItemAsync(OrderItem item);
        Task<Order?> GetOrderByReferenceNumberAsync(string referenceNumber);
        Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status, int page = 1, int pageSize = 10);
        Task<bool> UpdateOrderPaymentStatusAsync(int orderId, PaymentStatus paymentStatus);
    }
}
