using AutoMapper;
using Berryfy.Application.Dtos.PaymentDtos;
using Berryfy.Domain.Entities.PaymentEntities;

namespace Berryfy.Application.Mapping
{
    public class PaymentMapping : Profile
    {
        public PaymentMapping()
        {
            CreateMap<Payment, PaymentDto>().ReverseMap();
        }
    }
}
