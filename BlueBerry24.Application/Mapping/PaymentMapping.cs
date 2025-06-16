using AutoMapper;
using BlueBerry24.Application.Dtos.PaymentDtos;
using BlueBerry24.Domain.Entities.PaymentEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
