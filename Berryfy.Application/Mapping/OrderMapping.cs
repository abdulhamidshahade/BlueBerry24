using AutoMapper;
using Berryfy.Application.Dtos.OrderDtos;
using Berryfy.Domain.Entities.OrderEntities;

namespace Berryfy.Application.Mapping
{
    public class OrderMapping : Profile
    {
        public OrderMapping()
        {
            CreateMap<Order, OrderDto>()
           .ReverseMap();

            CreateMap<OrderItem, OrderItemDto>()
                .ReverseMap();
        }
    }
}
