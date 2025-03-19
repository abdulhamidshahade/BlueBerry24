using AutoMapper;
using BlueBerry24.Services.StockAPI.Models;
using BlueBerry24.Services.StockAPI.Models.DTOs;

namespace BlueBerry24.Services.StockAPI.Halpers.Mapping
{
    public class StockAutoMapper : Profile
    {
        public StockAutoMapper()
        {
            CreateMap<Stock, StockDto>().ReverseMap();
            CreateMap<Stock, CreateStockDto>().ReverseMap();
        }
    }
}
