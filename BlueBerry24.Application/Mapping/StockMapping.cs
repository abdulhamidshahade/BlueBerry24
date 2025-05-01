using AutoMapper;
using BlueBerry24.Application.Dtos.StockDtos;
using BlueBerry24.Domain.Entities.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Mapping
{
    class StockMapping : Profile
    {
        public StockMapping()
        {
            CreateMap<StockDto, CreateStockDto>().ReverseMap();
            CreateMap<StockDto, UpdateStockDto>().ReverseMap();
            CreateMap<StockDto, Stock>().ReverseMap();
        }
    }
}
