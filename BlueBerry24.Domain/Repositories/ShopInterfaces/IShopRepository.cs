using BlueBerry24.Domain.Entities.ShopEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Domain.Repositories.ShopInterfaces
{
    public interface IShopRepository
    {
        Task<Shop> GetShopAsync(int id);
        Task<Shop> UpdateShopAsync(Shop shop);
    }
}
