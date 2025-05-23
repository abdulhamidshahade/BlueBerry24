using BlueBerry24.Application.Dtos.ShopDtos;
using BlueBerry24.Domain.Entities.ShopEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Services.Interfaces.ShopServiceInterfaces
{
    public interface IShopService
    {
        Task<Shop> GetShopAsync(int id);
        Task<ShopDto> UpdateShopAsync(int id, UpdateShopDto shop);
    }
}
