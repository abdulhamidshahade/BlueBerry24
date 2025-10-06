using BlueBerry24.Domain.Constants;

namespace BlueBerry24.Application.Dtos.OrderDtos
{
    public class UpdateOrderStatusRequest
    {
        public OrderStatus NewStatus { get; set; }
    }

}
