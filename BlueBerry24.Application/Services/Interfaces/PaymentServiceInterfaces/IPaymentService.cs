using BlueBerry24.Application.Dtos;
using BlueBerry24.Application.Dtos.PaymentDtos;
using BlueBerry24.Domain.Constants;

namespace BlueBerry24.Application.Services.Interfaces.PaymentServiceInterfaces
{
    public interface IPaymentService
    {
        Task<ResponseDto<PaymentResponseDto>> ProcessPaymentAsync(CreatePaymentDto createPaymentDto, int? userId, string? sessionId);
        Task<ResponseDto<PaymentDto>> GetPaymentByIdAsync(int id);
        Task<ResponseDto<PaymentDto>> GetPaymentByTransactionIdAsync(string transactionId);
        Task<ResponseDto<PaymentDto>> GetPaymentByOrderIdAsync(int orderId);
        Task<ResponseDto<IEnumerable<PaymentDto>>> GetAllPaymentsAsync();
        Task<ResponseDto<IEnumerable<PaymentDto>>> GetPaymentsByUserIdAsync(int userId);
        Task<ResponseDto<IEnumerable<PaymentDto>>> GetPaymentsByStatusAsync(PaymentStatus status);
        Task<ResponseDto<IEnumerable<PaymentDto>>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<ResponseDto<IEnumerable<PaymentDto>>> GetPaginatedPaymentsAsync(int pageNumber, int pageSize);
        Task<ResponseDto<IEnumerable<PaymentDto>>> GetPaginatedPaymentsByUserIdAsync(int userId, int pageNumber, int pageSize);
        Task<ResponseDto<PaymentResponseDto>> UpdatePaymentStatusAsync(int id, PaymentStatus status, string? notes = null);
        Task<ResponseDto<PaymentResponseDto>> RefundPaymentAsync(int id, decimal? refundAmount = null, string? reason = null);
        Task<ResponseDto<bool>> DeletePaymentAsync(int id);
        Task<ResponseDto<int>> GetTotalPaymentCountAsync();
        Task<ResponseDto<int>> GetPaymentCountByUserIdAsync(int userId);
        Task<ResponseDto<int>> GetPaymentCountByStatusAsync(PaymentStatus status);
        Task<ResponseDto<decimal>> GetTotalAmountByUserIdAsync(int userId);
        Task<ResponseDto<decimal>> GetTotalAmountByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<ResponseDto<IEnumerable<PaymentDto>>> SearchPaymentsAsync(string searchTerm, int pageNumber, int pageSize);
        Task<ResponseDto<PaymentResponseDto>> VerifyPaymentWithProviderAsync(string transactionId);
        Task<string> GenerateTransactionIdAsync();
    }
}
