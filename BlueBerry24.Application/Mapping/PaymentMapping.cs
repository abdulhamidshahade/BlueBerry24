using AutoMapper;
using BlueBerry24.Application.Dtos.PaymentDtos;
using BlueBerry24.Domain.Entities.PaymentEntities;

namespace BlueBerry24.Application.Mapping
{
    public class PaymentMapping : Profile
    {
        public PaymentMapping()
        {
            CreateMap<Payment, PaymentDto>().ReverseMap();
        }
    }
}
