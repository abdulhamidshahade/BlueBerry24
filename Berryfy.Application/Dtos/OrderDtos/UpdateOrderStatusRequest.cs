using Berryfy.Domain.Constants;

namespace Berryfy.Application.Dtos.OrderDtos
{
    public class UpdateOrderStatusRequest
    {
        public OrderStatus NewStatus { get; set; }
    }

}
