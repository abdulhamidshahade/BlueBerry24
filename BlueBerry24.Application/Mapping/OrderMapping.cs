using AutoMapper;
using BlueBerry24.Application.Dtos.OrderDtos;
using BlueBerry24.Domain.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
