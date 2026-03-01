using BlueBerry24.Domain.Constants;
using BlueBerry24.Domain.Entities.OrderEntities;
using BlueBerry24.Domain.Repositories;
using BlueBerry24.Domain.Repositories.OrderInterfaces;
using BlueBerry24.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlueBerry24.Infrastructure.Repositories.OrderConcretes
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public OrderRepository(ApplicationDbContext context,
                               IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        //TODO to fix
        public async Task<List<Order>> GetUserOrdersAsync(int userId, int page = 1, int pageSize = 10)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Order>> GetAllOrdersAsync(int page = 1, int pageSize = 50)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .OrderByDescending(o => o.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking() // Optimize for read-only operations
                .ToListAsync();
        }


        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null) return false;

            order.Status = newStatus;
            order.UpdatedAt = DateTime.UtcNow;

            switch (newStatus)
            {
                case OrderStatus.Completed:
                    order.CompletedAt = DateTime.UtcNow;
                    break;
                case OrderStatus.Cancelled:
                    order.CancalledAt = DateTime.UtcNow;
                    break;
            }

            _context.Orders.Update(order);
            return await _context.SaveChangesAsync() > 0;
        }

        public Task<string> GenerateUniqueReferenceNumberAsync()
        {
            var uniqueRef = $"ORD-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
            return Task.FromResult(uniqueRef);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            var createdOrder = await _context.Orders.AddAsync(order);

            if (await _unitOfWork.SaveDbChangesAsync())
            {
                return order;
            }

            return null;
        }

        public async Task<OrderItem> CreateOrderItemAsync(OrderItem item)
        {
            var createdItem = await _context.OrderItems.AddAsync(item);

            if (await _unitOfWork.SaveDbChangesAsync())
            {
                return item;
            }

            return null;
        }

        public async Task<Order?> GetOrderByReferenceNumberAsync(string referenceNumber)
        {
            if (string.IsNullOrWhiteSpace(referenceNumber))
                return null;

            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .Include(o => o.Cart)
                .FirstOrDefaultAsync(o => o.ReferenceNumber == referenceNumber);
        }

        public async Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status, int page = 1, int pageSize = 10)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .Where(o => o.Status == status)
                .OrderByDescending(o => o.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<bool> UpdateOrderPaymentStatusAsync(int orderId, PaymentStatus paymentStatus)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(v => v.Id == orderId);
            
            if(paymentStatus == PaymentStatus.Completed)
            {
                order.isPaid = true;
            }
            else
            {
                order.isPaid = false;
            }

            _context.Orders.Update(order);
            return await _unitOfWork.SaveDbChangesAsync();
        }

        public async Task<Order?> GetOrderByCartIdAsync(int cartId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.CartId == cartId && o.Status == OrderStatus.Pending)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            return await _unitOfWork.SaveDbChangesAsync();
        }

        public async Task<bool> DeleteOrderItemsAsync(int orderId)
        {
            var orderItems = await _context.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync();
            
            if (orderItems.Any())
            {
                _context.OrderItems.RemoveRange(orderItems);
                return await _unitOfWork.SaveDbChangesAsync();
            }
            
            return true;
        }
    }
}
