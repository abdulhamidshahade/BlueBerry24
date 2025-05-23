using BlueBerry24.Domain.Entities.ShopEntities;
using BlueBerry24.Domain.Repositories;
using BlueBerry24.Domain.Repositories.ShopInterfaces;
using BlueBerry24.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Infrastructure.Repositories.ShopConcretes
{
    public class ShopRepository : IShopRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public ShopRepository(ApplicationDbContext context,
                              IUnitOfWork unitOfWork)
        {
            _context = context;
        }

        public async Task<Shop> GetShopAsync(int id)
        {
            return await _context.Shops.
                Where(i => i.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Shop> UpdateShopAsync(Shop shop)
        {
            _context.Shops.Update(shop);
            
            if(await _unitOfWork.SaveDbChangesAsync())
            {
                return shop;
            }

            return null;
        }
    }
}
