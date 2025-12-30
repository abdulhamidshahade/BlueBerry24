using BlueBerry24.Domain.Constants;
using BlueBerry24.Domain.Entities.PaymentEntities;

namespace BlueBerry24.Domain.Repositories.PaymentInterfaces
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetByIdAsync(int id);
        Task<Payment?> GetByTransactionIdAsync(string transactionId);
        Task<Payment?> GetByOrderIdAsync(int orderId);
        Task<IEnumerable<Payment>> GetAllAsync();
        Task<IEnumerable<Payment>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status);
        Task<IEnumerable<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Payment>> GetPaginatedAsync(int pageNumber, int pageSize);
        Task<IEnumerable<Payment>> GetPaginatedByUserIdAsync(int userId, int pageNumber, int pageSize);
        Task<Payment> CreateAsync(Payment payment);
        Task<Payment> UpdateAsync(Payment payment);
        Task<bool> DeleteAsync(int id);
        Task<int> GetTotalCountAsync();
        Task<int> GetCountByUserIdAsync(int userId);
        Task<int> GetCountByStatusAsync(PaymentStatus status);
        Task<decimal> GetTotalAmountByUserIdAsync(int userId);
        Task<decimal> GetTotalAmountByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Payment>> SearchAsync(string searchTerm, int pageNumber, int pageSize);
    }
}
