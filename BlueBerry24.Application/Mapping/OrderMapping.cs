using AutoMapper;
using BlueBerry24.Application.Dtos.OrderDtos;
using BlueBerry24.Domain.Entities.OrderEntities;

namespace BlueBerry24.Application.Mapping
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
